using System;
using System.Data;
using System.Linq;

namespace Rebec
{
    internal static class DataTableExtension
    {
        public static string ToHtml(this DataRow dataRow)
        {
            var toReturn = $"<tr>{Environment.NewLine}";

            toReturn = dataRow.ItemArray.Aggregate(toReturn,
                (current, o) => current + $"\t<td>{o}</td>{Environment.NewLine}");

            toReturn += "</tr>";

            return toReturn;
        }

        public static string ToHtml(this DataColumn dataRow)
        {
            return $"<th>{dataRow}</th>";
        }
    }
}