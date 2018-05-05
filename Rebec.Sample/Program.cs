using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Bogus;

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

            var reportBuilder = new ReportBuilder().FromTemplate(ReportType.Invoice)
                .WithCssUrl("https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.1/css/bulma.min.css")
                .WithTableClass("table is-bordered")
                .WithTableTitleClass("title")
                .WithTableTitle("Pessoas");

            Console.WriteLine("Started");

            var stopWatch = Stopwatch.StartNew();

            reportBuilder.Build(enumerable).SaveAsPdf("testing.pdf").SaveAsHtml("test.html");

            stopWatch.Stop();

            Console.WriteLine($"Success - {stopWatch.Elapsed}");
        }
    }
}