using Rebec.Builders;

namespace Rebec.Interfaces
{
    public interface IReportResult
    {
        ReportBuilder Builder { get; }

        string Html { get; }
    }
}