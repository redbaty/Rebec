using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Bogus;
using Rebec.Builders;

namespace Rebec.Sample
{
    internal class Person
    {
        public string FirstName { get; set; }

        [DisplayName("Fak")]
        public string LastName { get; set; }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var enumerable = new Faker<Person>().RuleFor(i => i.FirstName, f => f.Person.FirstName)
                .RuleFor(i => i.LastName, f => f.Person.LastName).Generate(100).ToList();

            new ReportBuilder().Then(() => new TextBuilder().WithText("Hello world!")).Then(() => new TableBuilder()
                    .WithItems(enumerable).UseItemsPropertiesAsColumns()
                    .WithStyling(new BuilderStyle("table is-bordered"))).WithTitle("Fack me")
                .UseCss("https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.1/css/bulma.min.css").Build();
        }
    }
}