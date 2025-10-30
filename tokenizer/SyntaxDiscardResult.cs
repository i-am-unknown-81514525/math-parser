namespace math_parser.tokenizer
{
    public class SyntaxDiscardResult : ParseResult
    {
        public readonly string content;

        public SyntaxDiscardResult(string inner)
        {
            content = inner;
        }
    }
}