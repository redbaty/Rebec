using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using LazyCache;

namespace Rebec
{
    internal static class Globals
    {
        public static IAppCache Cache { get; } = new CachingService();
    }

    public static class EnumExtensions
    {
        public static string GetEnumDescription<T>(this T value) where T : struct, IConvertible
        {
            var fi = value.GetType().GetField(value.ToString(CultureInfo.CurrentCulture));

            var attributes =
                (DescriptionAttribute[]) fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString(CultureInfo.CurrentCulture);
        }
    }

    public static class ListExtensions
    {
        public static DataTable ToDataTable(this IEnumerable data)
        {
            var type = data.GetType().GetGenericArguments().FirstOrDefault();

            if (type == null)
            {
                throw new InvalidOperationException("Could not find the IEnumerable object type");
            }

            var properties = Globals.Cache.GetOrAdd($"PropertiesFor->{type.FullName}", () => type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance));

            var table = new DataTable();
            foreach (var prop in properties)
            {
                table.Columns.Add(
                    GetPropertyName(prop), Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (var item in data)
            {
                var row = table.NewRow();
                foreach (var prop in properties)
                    row[GetPropertyName(prop)] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            return table;
        }

        private static string GetPropertyName(MemberInfo prop)
        {
            return Globals.Cache.GetOrAdd($"PropertyName->{prop.Name}->{prop.MetadataToken}", () =>
                prop.GetCustomAttribute<DisplayNameAttribute>() is DisplayNameAttribute attribute
                    ? attribute.DisplayName
                    : prop.Name);
        }
    }

    internal static class DataTableExtension
    {
        public static string ToHtml(this DataRow dataRow)
        {
            var toReturn = $"<tr>{Environment.NewLine}";

            toReturn = dataRow.ItemArray.Aggregate(toReturn,
                (current, o) => current + $"\t<td>{o}</td>{Environment.NewLine}");

            toReturn += "</tr>";

            return toReturn;
        }

        public static string ToHtml(this DataColumn dataRow)
        {
            return $"<th>{dataRow}</th>";
        }
    }

    public interface IReportBuilderContext
    {
        string TableTitle { get; }

        string Header { get; }

        string TableClass { get; }
    }

    internal static class ReportTypeExtensions
    {
        public static Stream GetResourceStream(this ReportType templateType)
        {
            var assembly = Globals.Cache.GetOrAdd("ResAssembly", () => Assembly.GetAssembly(typeof(ReportBuilder)));
            var resources = assembly.GetManifestResourceNames();
            var resourceName = resources.FirstOrDefault(i => i.EndsWith(templateType.GetEnumDescription()));

            if (resourceName == null)
            {
                throw new InvalidOperationException(
                    $"Unable to find the enum's file, please report this to the developer. Enum: {templateType}");
            }

            var stream = assembly.GetManifestResourceStream(resourceName);
            return stream;
        }
    }

    public enum ReportType
    {
        [Description("InvoiceTemplate.html")]
        Invoice
    }

    public class ReportBuilder : IReportBuilderContext
    {
        private string Html { get; set; }

        private string Css { get; set; }

        public ReportBuilder FromTemplate(ReportType templateType)
        {
            using (var readStream = new StreamReader(templateType.GetResourceStream() ??
                                                     throw new InvalidOperationException("Resource stream invalid")))
                Html = readStream.ReadToEnd();

            return this;
        }

        public ReportBuilder WithTableClass(string className)
        {
            TableClass = className;
            return this;
        }

        public ReportBuilder WithHeader(string header)
        {
            Header = header;
            return this;
        }

        public ReportBuilder WithTableTitle(string title)
        {
            TableTitle = title;
            return this;
        }

        public ReportBuilder WithCssFile(string filePath)
        {
            return WithCssFile(new FileInfo(filePath));
        }

        public ReportBuilder WithCssFile(FileInfo file)
        {
            using (var streamReader = file.OpenText())
            {
                Css += streamReader.ReadToEnd();
            }

            return this;
        }

        public ReportBuilder WithCssUrl(string cssUrl)
        {
            var wb = new WebClient();
            var css = wb.DownloadString(cssUrl);
            Css += css;
            return this;
        }

        public ReportBuilder WithCss(string css)
        {
            Css += css;
            return this;
        }

        public void Build(IEnumerable objects, Stream buildTo)
        {
            if (Html == null)
            {
                throw new InvalidOperationException("Please select a template first using FromTemplate");
            }

            var dTable = objects.ToDataTable();
            ReplaceOnTemplate("css", Css);
            ReplaceOnTemplate("columns", GetColumns(dTable));
            ReplaceOnTemplate("body", GetRows(dTable));
            ReplaceOnTemplate("subject", Header);
            ReplaceOnTemplate("title", TableTitle);


            HtmlConverter.ConvertToPdf(Html, buildTo);
        }

        private static string GetRows(DataTable dTable) => dTable.Rows.Cast<DataRow>().Select(i => i.ToHtml())
            .Aggregate((current, o) => current + $"{o}{Environment.NewLine}");

        private static string GetColumns(DataTable dTable) => dTable.Columns.Cast<DataColumn>().Select(i => i.ToHtml())
            .Aggregate((current, o) => current + $"{o}{Environment.NewLine}");

        private void ReplaceOnTemplate(string key, string value)
        {
            Html = Html.Replace($"{{{{{key.ToUpper()}}}}}", value);
        }

        /// <inheritdoc />
        public string TableTitle { get; private set; }

        /// <inheritdoc />
        public string Header { get; private set; }

        /// <inheritdoc />
        public string TableClass { get; private set; }
    }
}