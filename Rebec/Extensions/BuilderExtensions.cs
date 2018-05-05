using System;
using System.IO;
using iText.Html2pdf;
using iText.Kernel.Pdf;

namespace Rebec
{
    public static class BuilderExtensions
    {
        public static IBuilderResult SaveAsHtml(this IBuilderResult result, string path)
        {
            return SaveAsHtml(result, new FileInfo(path));
        }

        public static IBuilderResult SaveAsHtml(this IBuilderResult result, FileInfo fileInfo)
        {
            if(fileInfo.Exists)
                File.WriteAllText(fileInfo.FullName, string.Empty);
            
            using (var fileStream = new StreamWriter(fileInfo.OpenWrite()))
            {
                fileStream.Write(result.Content);                
            }
            
            return result;
        }

        public static IBuilderResult SaveAsPdf(this IBuilderResult result, string path)
        {
            return SaveAsPdf(result, new FileInfo(path));
        }

        public static IBuilderResult SaveAsPdf(this IBuilderResult result, FileInfo fileInfo)
        {
            HtmlConverter.ConvertToPdf(result.Content, new PdfWriter(fileInfo));
            return result;
        }
    }
}