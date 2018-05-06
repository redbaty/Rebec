using Rebec.Interfaces;

namespace Rebec.Builders
{
    internal class ReportResult : IReportResult
    {
        public ReportResult(string html, ReportBuilder builder)
        {
            Html = html;
            Builder = builder;
        }

        public string Html { get; }
        public ReportBuilder Builder { get; }
    }
}