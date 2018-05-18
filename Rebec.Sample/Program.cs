using System;
using System.Linq;
using Bogus;
using Rebec.Builders;
using Rebec.Extensions;
using Rebec.Models;
using Rebec.Representations;

namespace Rebec.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var people = new Faker<Person>().RuleFor(i => i.FirstName, f => f.Person.FirstName)
                .RuleFor(i => i.LastName, f => f.Person.LastName).Generate(100).ToList();

            var x = new ReportBuilder()
                .Then(() => new TextBuilder().WithText("Pedido nº 500").WithStyle(new BuilderStyle("title")).WithStyle(new BuilderStyle("font-size : 150px;",true)).WithStyle(new BuilderStyle("font-size : 3000px;", true)))
                .Then(() => new TextBuilder().WithText("Uniferso dos paravusos").WithStyle(new BuilderStyle("subtitle")))
                .Then(() => new TextBuilder().WithText($"Data de emissão: {DateTime.UtcNow : dd-mm-yyyy}")).WithCss("font-size: 0.5em")
                .Then<DividerBuilder>()
                .Then(() => new TableBuilder()
                    .WithItems(people).UseItemsPropertiesAsColumns()
                    .WithComputedColumn<Person>("SummedName", person => $"{person.FirstName}{person.LastName}")
                    .WithComputedColumn<Person>("ReversedSummedName", person => $"{person.LastName}{person.FirstName}")
                    .WithAdditionalRow(new TableRowBuilder()
                        .WithData(new TableDataRepresentation("Total", "3").Bold(), new TableDataRepresentation("R$56,00")).Build())
                    .WithStyling(new BuilderStyle("table is-bordered")))
                .WithTitle("Fack me")
                .TryUseCss("https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.1/css/bulma.min.css").Build().Result;


                x.SaveAsPdf("test.pdf");
        }
    }
}