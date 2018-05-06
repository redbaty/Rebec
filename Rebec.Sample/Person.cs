using System.ComponentModel;

namespace Rebec.Sample
{
    internal class Person
    {
        public string FirstName { get; set; }

        [DisplayName("Fak")]
        public string LastName { get; set; }
    }
}