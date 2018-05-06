using Rebec.Interfaces;

namespace Rebec.Builders
{
    public class BuilderStyle : IBuilderStyle
    {
        public BuilderStyle(string @class)
        {
            Class = @class;
        }

        public string Class { get; }
    }
}