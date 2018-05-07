using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using Rebec.Interfaces;

namespace Rebec.Builders
{
    public class ReportBuilder : IReportBuilderContext
    {
        public IList<IBuilder> Builders { get; } = new List<IBuilder>();

        public string Css { get; private set; }

        public string Title { get; private set; }

        public ReportBuilder WithTitle(string title)
        {
            Title = title;
            return this;
        }

        public ReportBuilder TryUseCss(string url)
        {
            try
            {
                Css = new WebClient().DownloadString(url);
            }
            catch
            {
                // ignored
            }

            return this;
        }

        public ReportBuilder UseCss(string url)
        {
            Css = new WebClient().DownloadString(url);
            return this;
        }

        public ReportBuilder WithCss(string css)
        {
            Css = css;
            return this;
        }

        public ReportBuilder Then<T>() where T : IBuilder
        {
            Builders.Add(Activator.CreateInstance<T>());
            return this;
        }

        public ReportBuilder Then<T>(Func<T> build) where T : IBuilder
        {
            Builders.Add(build.Invoke());
            return this;
        }

        public Task<IReportResult> Build()
        {
            return Task.Run(async () =>
            {
                var document = await BrowsingContext.New().OpenNewAsync();

                if (document.Head is IHtmlHeadElement headElement)
                {
                    AddHeader(document, headElement);
                    AddCss(document, headElement);
                }

                foreach (var builder in Builders)
                    document.Body.AppendChild(builder.Build());

                return new ReportResult(document.DocumentElement.OuterHtml, this) as IReportResult;
            });
        }

        private void AddCss(IDocument document, INode htmlHeadElement)
        {
            var styleElement = document.CreateElement<IHtmlStyleElement>();
            styleElement.InnerHtml = Css;
            htmlHeadElement.AppendElement(styleElement);
        }

        private void AddHeader(IDocument document, INode htmlHeadElement)
        {
            var titleElement = document.CreateElement<IHtmlTitleElement>();
            titleElement.InnerHtml = Title;
            htmlHeadElement.AppendElement(titleElement);
        }
    }
}