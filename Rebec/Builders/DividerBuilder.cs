using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using Rebec.Interfaces;

namespace Rebec.Builders
{
    public class DividerBuilder : IBuilder
    {
        /// <inheritdoc />
        public IElement Build()
        {
            var parser = new HtmlParser();
            var document = parser.Parse(string.Empty);
            return document.CreateElement<IHtmlHrElement>();
        }
    }
}