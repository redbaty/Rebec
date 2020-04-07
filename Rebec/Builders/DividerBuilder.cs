using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Rebec.Interfaces;

namespace Rebec.Builders
{
    public class DividerBuilder : IBuilder
    {
        /// <inheritdoc />
        public IElement Build()
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(string.Empty);
            return document.CreateElement<IHtmlHrElement>();
        }
    }
}