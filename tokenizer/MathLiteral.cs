namespace math_parser.tokenizer
{
    public class MathLiteralResult : MathAtomResult
    {
        public readonly string literal;

        public MathLiteralResult(string literal)
        {
            this.literal = literal;
        }

        public override string ToString() => ToString(0);
        public string ToString(int indent) => $"{ParseResultExtensions.Indent(indent)}MathLiteralResult: '{literal}'\n";
    }
    public class MathLiteral : Literal<MathLiteralResult>
    {
        public MathLiteral(string value) : base(value) {}

        public override MathLiteralResult Constructor(string content)
        {
            return new MathLiteralResult(content);
        }
    }
}