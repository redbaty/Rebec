using System.ComponentModel;
using System.IO;
using System.Linq;
using Bogus;
using iText.Html2pdf;
using iText.Kernel.Pdf;

namespace Rebec.Sample
{
    class Person
    {
        public string FirstName { get; set; }

        [DisplayName("Fak")]
        public string LastName { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var x12312 = new ReportBuilder().FromTemplate(ReportType.Invoice)
                .WithCssUrl("https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.1/css/bulma.min.css")
                .WithTableClass("table");
            var res = new FileInfo("vamoMataORonaldo.pdf");
            using (var fileStream = res.OpenWrite())
            {
                x12312.Build(new Faker<Person>().RuleFor(i => i.FirstName, f => f.Person.FirstName)
                    .RuleFor(i => i.LastName, f => f.Person.LastName).Generate(500), fileStream);
            }
        }
    }
}