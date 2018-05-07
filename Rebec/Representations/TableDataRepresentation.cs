namespace Rebec.Representations
{
    public class TableDataRepresentation
    {
        public string Content { get; private set; }

        public string Span { get; }

        public TableDataRepresentation(string content) : this(content, null)
        {
        }

        public TableDataRepresentation(string content, string span)
        {
            Content = content;
            Span = span;
        }

        public TableDataRepresentation Bold()
        {
            Content = $"<b>{Content}</b>";
            return this;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"\t<td{(string.IsNullOrEmpty(Span) ? "" : $" colspan=\"{Span}\"")}>{Content}</td>";
        }
    }
}