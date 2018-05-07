using Rebec.Interfaces;

namespace Rebec.Builders
{
    internal class ReportResult : IReportResult
    {
        public ReportBuilder Builder { get; }

        public string Html { get; }

        public ReportResult(string html, ReportBuilder builder)
        {
            Html = html;
            Builder = builder;
        }
    }
}