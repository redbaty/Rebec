namespace Rebec.Interfaces
{
    public interface ITextBuilderContext
    {
        string Text { get; }
        IBuilderStyle Style { get; }
    }
}