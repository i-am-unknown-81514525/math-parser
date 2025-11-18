namespace math_parser.tokenizer
{
    public class MathLiteralResult : MathAtomResult
    {
        public readonly string Literal;

        public MathLiteralResult(string literal)
        {
            Literal = literal;
        }

        public override string ToString() => ToString(0);
        public virtual string ToString(int indent) => $"{ParseResultExtensions.Indent(indent)}MathLiteralResult: '{Literal}'\n";
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