namespace math_parser.tokenizer
{
    public class SyntaxDiscardResult : ParseResult
    {
        public readonly string content;

        public SyntaxDiscardResult(string inner)
        {
            content = inner;
        }

        public override string ToString() => ToString(0);
        public string ToString(int indent)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return "";
            }

            string displayContent = content.Replace("\n", "\\n");
            return $"{ParseResultExtensions.Indent(indent)}SyntaxDiscardResult: '{displayContent}'\n";
        }
    }
}