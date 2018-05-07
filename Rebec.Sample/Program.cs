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

            new ReportBuilder()
                .Then(() => new TextBuilder().WithText("Hello world!").WithStyle(new BuilderStyle("title")))
                .Then(() => new TextBuilder().WithText("O Ronaldo é gay").WithStyle(new BuilderStyle("subtitle")))
                .Then<DividerBuilder>()
                .Then(() => new TableBuilder()
                    .WithItems(people).UseItemsPropertiesAsColumns()
                    .WithComputedColumn<Person>("SummedName", person => $"{person.FirstName}{person.LastName}")
                    .WithComputedColumn<Person>("ReversedSummedName", person => $"{person.LastName}{person.FirstName}")
                    .WithAdditionalRow(new TableRowBuilder()
                        .WithData(new TableDataRepresentation("Total", "3").Bold(), new TableDataRepresentation("R$56,00")).Build())
                    .WithStyling(new BuilderStyle("table is-bordered")))
                .WithTitle("Fack me")
                .TryUseCss("https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.1/css/bulma.min.css").Build().Result
                .SaveAsPdf("test.pdf");
        }
    }
}