using Rebec.Interfaces;

namespace Rebec.Models
{
    public class BuilderStyle : IBuilderStyle
    {
        public string Class { get; }
        public bool IsInline { get; }

        public BuilderStyle(string @class, bool isInline = false)
        {
            Class = @class;
            IsInline = isInline;
        }
    }
}