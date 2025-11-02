namespace math_parser.tokenizer
{
    public class OppoSignResult : MathLiteralResult
    {
        public OppoSignResult() : base("-") {}

        public override string ToString() => ToString(0);
        public new string ToString(int indent) => $"{ParseResultExtensions.Indent(indent)}OppoSignResult\n";
    }

    public class OppoSign : Group<MathLiteralResult, OppoSignResult>
    {
        public OppoSign() : base(new MathLiteral("-")) {}

        public override OppoSignResult Parse(CharacterStream stream)
        {
            inner_token.Parse(stream);
            return new OppoSignResult();
        }
    }
}