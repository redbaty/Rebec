using Rebec.Builders;

namespace Rebec.Interfaces
{
    public interface IReportResult
    {
        string Html { get; }
        ReportBuilder Builder { get; }
    }
}