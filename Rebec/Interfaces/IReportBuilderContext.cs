using System.Collections.Generic;

namespace Rebec.Interfaces
{
    public interface IReportBuilderContext
    {
        IList<IBuilder> Builders { get; }

        HashSet<string> Css { get; }

        string Title { get; }
    }
}