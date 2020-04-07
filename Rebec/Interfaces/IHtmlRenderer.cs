using System.Threading.Tasks;

namespace Rebec.Interfaces
{
    public interface IHtmlRenderer
    {
        public Task<byte[]> RenderHtml(IReportResult reportResult);
    }
}