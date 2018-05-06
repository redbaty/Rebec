using System.Collections.Generic;

namespace Rebec.Interfaces
{
    public interface IReportBuilderContext
    {
        IList<IBuilder> Builders { get; }
        string Title { get; }
        string Css { get; }
    }
}