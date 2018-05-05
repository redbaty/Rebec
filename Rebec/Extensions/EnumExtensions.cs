using System;
using System.ComponentModel;
using System.Globalization;

namespace Rebec
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription<T>(this T value) where T : struct, IConvertible
        {
            var fi = value.GetType().GetField(value.ToString(CultureInfo.CurrentCulture));

            var attributes =
                (DescriptionAttribute[]) fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString(CultureInfo.CurrentCulture);
        }
    }
}