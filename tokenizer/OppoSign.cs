namespace math_parser.tokenizer
{
    public class OppoSignResult : MathLiteralResult
    {
        public OppoSignResult() : base("-") {}

        public override string ToString() => ToString(0);
        public override string ToString(int indent) => $"{ParseResultExtensions.Indent(indent)}OppoSignResult\n";
    }

    public class OppoSign : Group<MathLiteralResult, OppoSignResult>
    {
        public OppoSign() : base(new MathLiteral("-")) {}

        public override OppoSignResult Parse(CharacterStream stream)
        {
            InnerToken.Parse(stream);
            return new OppoSignResult();
        }
    }
}