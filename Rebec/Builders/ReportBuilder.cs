using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;

namespace Rebec
{
    public interface IBuilderResult
    {
        string Content { get; }
    }

    internal class BuilderResult : IBuilderResult
    {
        public BuilderResult(string content)
        {
            Content = content;
        }

        public string Content { get; }
    }

    public class ReportBuilder : IReportBuilderContext
    {
        private string Html { get; set; }

        private string Css { get; set; }

        /// <inheritdoc />
        public string TableTitle { get; private set; }

        /// <inheritdoc />
        public string Header { get; private set; }

        /// <inheritdoc />
        public string TableClass { get; private set; }

        /// <inheritdoc />
        public string TableTitleClass { get; private set; }

        public ReportBuilder FromTemplate(ReportType templateType)
        {
            using (var readStream = new StreamReader(templateType.GetResourceStream() ??
                                                     throw new InvalidOperationException("Resource stream invalid")))
            {
                Html = readStream.ReadToEnd();
            }

            return this;
        }

        public ReportBuilder WithTableClass(string className)
        {
            TableClass = className;
            return this;
        }

        public ReportBuilder WithTableTitleClass(string className)
        {
            TableTitleClass = className;
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

        public IBuilderResult Build(IEnumerable objects)
        {
            if (Html == null) throw new InvalidOperationException("Please select a template first using FromTemplate");

            BuildTemplate(objects);
            return new BuilderResult(Html);
        }

        private void BuildTemplate(IEnumerable objects)
        {
            var dTable = objects.ToDataTable();
            ReplaceOnTemplate("css", Css);
            ReplaceOnTemplate("columns", GetColumns(dTable));
            ReplaceOnTemplate("body", GetRows(dTable));
            ReplaceOnTemplate("title", Header);
            ReplaceOnTemplate("TABLECLASS", TableClass);
            ReplaceOnTemplate("TABLETITLE", TableTitle);
            ReplaceOnTemplate("TABLETITLECLASS", TableTitleClass);
        }

        private static string GetRows(DataTable dTable)
        {
            return dTable.Rows.Cast<DataRow>().Select(i => i.ToHtml())
                .Aggregate((current, o) => current + $"{o}{Environment.NewLine}");
        }

        private static string GetColumns(DataTable dTable)
        {
            return dTable.Columns.Cast<DataColumn>().Select(i => i.ToHtml())
                .Aggregate((current, o) => current + $"{o}{Environment.NewLine}");
        }


        private void ReplaceOnTemplate(string key, string value)
        {
            Html = Html.Replace($"{{{{{key.ToUpper()}}}}}", value);
        }
    }
}