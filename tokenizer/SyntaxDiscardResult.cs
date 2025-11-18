namespace math_parser.tokenizer
{
    public class SyntaxDiscardResult : ParseResult
    {
        public readonly string Content;

        public SyntaxDiscardResult(string inner)
        {
            Content = inner;
        }

        public override string ToString() => ToString(0);
        public string ToString(int indent)
        {
            if (string.IsNullOrWhiteSpace(Content))
            {
                return "";
            }

            string displayContent = Content.Replace("\n", "\\n");
            return $"{ParseResultExtensions.Indent(indent)}SyntaxDiscardResult: '{displayContent}'\n";
        }
    }
}