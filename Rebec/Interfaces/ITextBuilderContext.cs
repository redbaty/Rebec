namespace Rebec.Interfaces
{
    public interface ITextBuilderContext
    {
        IBuilderStyle Style { get; }

        string Text { get; }
    }
}