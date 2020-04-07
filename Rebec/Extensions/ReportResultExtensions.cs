using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DinkToPdf;
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
            File.WriteAllText(path, reportResult.Html, Encoding.UTF8);
        }

        public static async Task SaveAsPdf<T>(this IReportResult reportResult, string path) where T: IHtmlRenderer
        {
            var rendererInstance = Activator.CreateInstance<T>();
            var retorno = await rendererInstance.RenderHtml(reportResult);
            
            var fileStream = File.OpenWrite(path);
            fileStream.Write(retorno);
        }

        public static Task SaveAsPdf<T>(this IReportResult reportResult, FileInfo fileInfo) where T : IHtmlRenderer
        {
            return SaveAsPdf<T>(reportResult, fileInfo.FullName);
        }
    }
}