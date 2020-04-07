using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Rebec.Interfaces;

namespace Rebec.Builders
{
    public class DivBuilder : IBuilder
    {
        public List<IBuilderStyle> Style { get; } = new List<IBuilderStyle>();
        
        public string Id { get; set; }
        
        public List<INode> Children { get; } = new List<INode>();

        public IElement Build()
        {
            var htmlDivElement = new HtmlParser().ParseDocument(string.Empty)
                .CreateElement<IHtmlDivElement>();

            if (!string.IsNullOrEmpty(Id))
            {
                htmlDivElement.Id = Id;
            }

            foreach (var child in Children)
            {
                htmlDivElement.AppendChild(child);
            }

            if (Style.Any(i => !i.IsInline))
                htmlDivElement.SetAttribute("class",
                    Style.Where(i => !i.IsInline).Select(i => i.Class).Aggregate((x, y) => $"{x};{y}"));

            if (Style.Any(i => i.IsInline))
                htmlDivElement.SetAttribute("style",
                    Style.Where(i => i.IsInline).Select(i => i.Class).Aggregate((x, y) => $"{x};{y}"));

            return htmlDivElement;
        }

        public DivBuilder WithStyle(IBuilderStyle style)
        {
            Style.Add(style);
            return this;
        }

        public DivBuilder WithChild(INode child)
        {
            Children.Add(child);
            return this;
        }
    }
}