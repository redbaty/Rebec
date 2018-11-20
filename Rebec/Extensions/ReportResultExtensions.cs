﻿using System.IO;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Rebec.Interfaces;

namespace Rebec.Extensions
{
    public static class ReportResultExtensions
    {
        public static void SaveAsHtml(this IReportResult reportResult, FileInfo fileInfo)
        {
            File.WriteAllText(fileInfo.FullName, reportResult.Html);
        }

        public static void SaveAsHtml(this IReportResult reportResult, string path)
        {
            File.WriteAllText(path, reportResult.Html);
        }

        public static void SaveAsPdf(this IReportResult reportResult, string path)
        {
            HtmlConverter.ConvertToPdf(reportResult.Html, new PdfWriter(path));
        }

        public static MemoryStream SaveAsPdfStream(this IReportResult reportResult)
        {
            var stream = new MemoryStream();
            HtmlConverter.ConvertToPdf(reportResult.Html, new PdfWriter(stream));
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
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