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

           
        }
    }
}