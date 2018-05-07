using System;
using System.Collections.Generic;
using System.Linq;
using Rebec.Interfaces;
using Rebec.Representations;

namespace Rebec.Builders
{
    public class TableRowBuilder : ITableRowContext
    {
        /// <inheritdoc />
        public IEnumerable<TableDataRepresentation> Data { get; private set; } = new List<TableDataRepresentation>();

        public string Build()
        {
            return $"<tr>{Environment.NewLine}" +
                   Data.Select(i => i.ToString()).Aggregate((x, y) => $"{x}{Environment.NewLine}{y}") +
                   $"{Environment.NewLine}</tr>";
        }

        public TableRowBuilder WithData(params TableDataRepresentation[] dataRepresentation)
        {
            Data = dataRepresentation;
            return this;
        }
    }
}