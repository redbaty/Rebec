using System.Collections.Generic;
using Rebec.Representations;

namespace Rebec.Interfaces
{
    public interface ITableRowContext
    {
        IEnumerable<TableDataRepresentation> Data { get; }
    }
}