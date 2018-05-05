using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using LazyCache;

namespace Rebec
{
    public static class ListExtensions
    {
        public static DataTable ToDataTable(this IEnumerable data)
        {
            var type = data.GetType().GetGenericArguments().FirstOrDefault();

            if (type == null) throw new InvalidOperationException("Could not find the IEnumerable object type");

            var properties = Globals.Cache.GetOrAdd($"PropertiesFor->{type.FullName}", () => type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance));

            var table = new DataTable();
            foreach (var prop in properties)
                table.Columns.Add(
                    GetPropertyName(prop), Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (var item in data)
            {
                var row = table.NewRow();
                foreach (var prop in properties)
                    row[GetPropertyName(prop)] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            return table;
        }

        private static string GetPropertyName(MemberInfo prop)
        {
            return Globals.Cache.GetOrAdd($"PropertyName->{prop.Name}->{prop.MetadataToken}", () =>
                prop.GetCustomAttribute<DisplayNameAttribute>() is DisplayNameAttribute attribute
                    ? attribute.DisplayName
                    : prop.Name);
        }
    }
}