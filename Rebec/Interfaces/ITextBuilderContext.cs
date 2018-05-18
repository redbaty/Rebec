using System.Collections.Generic;

namespace Rebec.Interfaces
{
    public interface ITextBuilderContext
    {
        List<IBuilderStyle> Style { get; }

        string Text { get; }
    }
}