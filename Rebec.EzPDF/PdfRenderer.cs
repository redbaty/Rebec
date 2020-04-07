using System.Threading.Tasks;
using EzPDF;
using Rebec.Interfaces;

namespace Rebec.EzPDF
{
    public class PdfRenderer : IHtmlRenderer
    {
        public Task<byte[]> RenderHtml(IReportResult reportResult)
        {
            var pdfRenderer = new HtmlRenderer();
            return pdfRenderer.Render(reportResult.Html);
        }
    }
}