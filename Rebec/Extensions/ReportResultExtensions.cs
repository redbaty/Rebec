using System.IO;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Rebec.Interfaces;

namespace Rebec.Extensions
{
    public static class ReportResultExtensions
    {
        public static void SaveAsHtml(this IReportResult reportResult, FileInfo fileInfo)
        {
            File.WriteAllText(reportResult.Html, fileInfo.FullName);
        }

        public static void SaveAsHtml(this IReportResult reportResult, string path)
        {
            File.WriteAllText(reportResult.Html, path);
        }

        public static void SaveAsPdf(this IReportResult reportResult, string path)
        {
            HtmlConverter.ConvertToPdf(reportResult.Html, new PdfWriter(path));
        }

        public static void SaveAsPdf(this IReportResult reportResult, FileInfo fileInfo)
        {
            SaveAsPdf(reportResult, fileInfo.FullName);
        }

        public static MemoryStream GetPdfStream(this IReportResult reportResult)
        {
            using (var stream = new MemoryStream())
            {
                HtmlConverter.ConvertToPdf(reportResult.Html, new PdfWriter(stream));
                return stream;
            }
        }
    }
}