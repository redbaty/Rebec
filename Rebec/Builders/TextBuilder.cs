using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using Rebec.Extensions;
using Rebec.Interfaces;

namespace Rebec.Builders
{
    public class TextBuilder : ITextBuilderContext, IBuilder
    {
        public IElement Build()
        {
            var htmlParagraphElement = new HtmlParser().Parse(string.Empty)
                .CreateElement<IHtmlParagraphElement>().WithInnerHtml(Text);

            if (!string.IsNullOrEmpty(Style?.Class))
                htmlParagraphElement.SetAttribute("class", Style.Class);

            return htmlParagraphElement;
        }

        public string Text { get; private set; }
        public IBuilderStyle Style { get; private set; }

        public TextBuilder WithText(string text)
        {
            Text = text;
            return this;
        }

        public TextBuilder WithStyle(IBuilderStyle style)
        {
            Style = style;
            return this;
        }
    }
}