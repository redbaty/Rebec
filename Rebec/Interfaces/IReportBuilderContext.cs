using System.Collections.Generic;

namespace Rebec.Interfaces
{
    public interface IReportBuilderContext
    {
        IList<IBuilder> Builders { get; }

        string Css { get; }

        string Title { get; }
    }
}