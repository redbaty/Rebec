using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Rebec.Extensions;
using Rebec.Interfaces;

namespace Rebec.Builders
{
    public class TextBuilder : ITextBuilderContext, IBuilder
    {
        public IElement Build()
        {
            var htmlParagraphElement = new HtmlParser().ParseDocument(string.Empty)
                .CreateElement<IHtmlParagraphElement>().WithInnerHtml(Text);

            if (Style.Any(i => !i.IsInline))
                htmlParagraphElement.SetAttribute("class",
                    Style.Where(i => !i.IsInline).Select(i => i.Class).Aggregate((x, y) => $"{x};{y}"));

            if (Style.Any(i => i.IsInline))
                htmlParagraphElement.SetAttribute("style",
                    Style.Where(i => i.IsInline).Select(i => i.Class).Aggregate((x, y) => $"{x};{y}"));

            return htmlParagraphElement;
        }

        public List<IBuilderStyle> Style { get; } = new List<IBuilderStyle>();

        public string Text { get; private set; }

        public TextBuilder WithText(string text)
        {
            Text = text;
            return this;
        }

        public TextBuilder WithStyle(IBuilderStyle style)
        {
            Style.Add(style);
            return this;
        }
    }
}