using Rebec.Interfaces;

namespace Rebec.Models
{
    public class BuilderStyle : IBuilderStyle
    {
        public string Class { get; }

        public BuilderStyle(string @class)
        {
            Class = @class;
        }
    }
}