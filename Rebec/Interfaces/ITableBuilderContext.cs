using System.Collections;
using System.Collections.Generic;
using Rebec.Representations;

namespace Rebec.Interfaces
{
    public interface ITableBuilderContext
    {
        ICollection<ColumnRepresentation> Columns { get; }

        ICollection Objects { get; }

        IBuilderStyle Style { get; }
    }
}