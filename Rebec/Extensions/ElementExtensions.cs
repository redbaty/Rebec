using AngleSharp.Dom;

namespace Rebec.Extensions
{
    public static class ElementExtensions
    {
        public static T WithInnerHtml<T>(this T item, string value) where T : IElement
        {
            item.InnerHtml = value;
            return item;
        }
    }
}