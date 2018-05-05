namespace Rebec
{
    public interface IReportBuilderContext
    {
        string TableTitle { get; }

        string Header { get; }

        string TableClass { get; }

        string TableTitleClass { get; }
    }
}