using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Rebec.Interfaces;
using Rebec.Models;

namespace Rebec.Builders
{
    public class ReportBuilder : IReportBuilderContext
    {
        public IList<IBuilder> Builders { get; } = new List<IBuilder>();

        public HashSet<string> Css { get; } = new HashSet<string>();

        public HashSet<string> HeaderScripts { get; } = new HashSet<string>();

        public HashSet<string> BodyScripts { get; } = new HashSet<string>();

        public string Title { get; private set; }

        public ReportBuilder WithTitle(string title)
        {
            Title = title;
            return this;
        }

        public ReportBuilder TryUseCss(string url)
        {
            using var client = CreateClient();
            var cssText = client.GetStringAsync(url).ContinueWith(t => t.IsCompletedSuccessfully ? t.Result : null)
                .Result;

            if (!string.IsNullOrEmpty(cssText))
                Css.Add(cssText);

            return this;
        }

        public ReportBuilder TryUseHeaderScript(string url)
        {
            using var client = CreateClient();
            var scriptText = client.GetStringAsync(url).ContinueWith(t => t.IsCompletedSuccessfully ? t.Result : null)
                .Result;

            if (!string.IsNullOrEmpty(scriptText))
                HeaderScripts.Add(scriptText);

            return this;
        }

        private static HttpClient CreateClient()
        {
            return new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });
        }

        public ReportBuilder TryUseBodyScript(string url)
        {
            using var client = CreateClient();
            var scriptText = client.GetStringAsync(url).ContinueWith(t => t.IsCompletedSuccessfully ? t.Result : null)
                .Result;

            if (!string.IsNullOrEmpty(scriptText))
                BodyScripts.Add(scriptText);

            return this;
        }


        public ReportBuilder WithCss(string css)
        {
            if (!string.IsNullOrEmpty(css))
                Css.Add(css);
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

                if (document.Head is { } headElement)
                {
                    AddHeader(document, headElement);
                    AddCss(document, headElement);
                    AddScripts(document, headElement, HeaderScripts);
                }

                foreach (var builder in Builders)
                    document.Body.AppendChild(builder.Build());

                AddScripts(document, document.Body, BodyScripts);
                return new ReportResult(document.DocumentElement.OuterHtml, this) as IReportResult;
            });
        }

        private static void AddScripts(IDocument document, INode htmlHeadElement, IEnumerable<string> scripts)
        {
            foreach (var script in scripts)
            {
                var scriptElement = document.CreateElement<IHtmlScriptElement>();
                scriptElement.InnerHtml = script;
                htmlHeadElement.AppendElement(scriptElement);
            }
        }


        private void AddCss(IDocument document, INode htmlHeadElement)
        {
            foreach (var cssText in Css)
            {
                var styleElement = document.CreateElement<IHtmlStyleElement>();
                styleElement.InnerHtml = cssText;
                htmlHeadElement.AppendElement(styleElement);
            }
        }

        private void AddHeader(IDocument document, INode htmlHeadElement)
        {
            var titleElement = document.CreateElement<IHtmlTitleElement>();
            titleElement.InnerHtml = Title;
            htmlHeadElement.AppendElement(titleElement);
        }
    }
}