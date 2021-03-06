﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Rebec.Builders;
using Rebec.Extensions;
using Rebec.EzPDF;
using Rebec.Models;
using Rebec.Representations;

namespace Rebec.Sample
{
    internal class Program
    {
        private static async Task Main()
        {
            var people = new Faker<Person>().RuleFor(i => i.FirstName, f => f.Person.FirstName)
                .RuleFor(i => i.LastName, f => f.Person.LastName).Generate(100).ToList();

            var stopwatch = Stopwatch.StartNew();

            var report = await new ReportBuilder()
                .Then(() => new DivBuilder()
                    .WithStyle(new BuilderStyle("control-section"))
                    .WithStyle(new BuilderStyle("height: 150px", true))
                    .WithChild(new DivBuilder {Id = "container"}.Build()))
                .Then(() => new TextBuilder().WithText("Pedido nº 500").WithStyle(new BuilderStyle("title"))
                    .WithStyle(new BuilderStyle("font-size : 200px !important;", true)))
                .Then(() => new TextBuilder().WithText("Uniferso dos paravusos")
                    .WithStyle(new BuilderStyle("subtitle")))
                .Then(() => new TextBuilder().WithText($"Data de emissão: {DateTime.UtcNow: dd-mm-yyyy}"))
                .WithCss("font-size: 32px")
                .Then<DividerBuilder>()
                .Then(() => new TableBuilder()
                    .WithItems(people).UseItemsPropertiesAsColumns()
                    .WithComputedColumn<Person>("SummedName", person => $"{person.FirstName}{person.LastName}")
                    .WithComputedColumn<Person>("ReversedSummedName", person => $"{person.LastName}{person.FirstName}")
                    .WithAdditionalRow(new TableRowBuilder()
                        .WithData(new TableDataRepresentation("Total", "3").Bold(),
                            new TableDataRepresentation("R$56,00")).Build())
                    .WithStyling(new BuilderStyle("table is-bordered")))
                .WithTitle("Report DEMO")
                .TryUseCss("https://cdnjs.cloudflare.com/ajax/libs/bulma/0.8.1/css/bulma.min.css")
                .Build();
            
            await report.SaveAsPdf<PdfRenderer>("report-demo.pdf");
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }
    }
}