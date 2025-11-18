namespace math_parser.tokenizer
{
    public class PotentialSpace : Group<RepeatListResult<SyntaxDiscardResult>, SyntaxDiscardResult>
    {
        public PotentialSpace() : base(
            new Repeat<SyntaxDiscardResult>(
                new Literal(" "),
                0,
                Amount.Unbound
            )
        )
        { }

        public override SyntaxDiscardResult Parse(CharacterStream stream)
        {
            CharacterStream cp = stream.Fork();
            InnerToken.Parse(cp);
            return new SyntaxDiscardResult(stream.TakeTo(cp));
        }
    }

    public class PotentialNewLine : Group<ParseResult, SyntaxDiscardResult>
    {
        public PotentialNewLine() : base(
            new TokenSequence<ParseResult>(
                new PotentialSpace(),
                new Or<ParseResult>(
                    new Literal("\r\n"),
                    new Literal("\n"),
                    new Literal("\r")
                ),
                new PotentialSpace()
            )
        )
        { }

        public override SyntaxDiscardResult Parse(CharacterStream stream)
        {
            CharacterStream cp = stream.Fork();
            InnerToken.Parse(cp);
            return new SyntaxDiscardResult(stream.TakeTo(cp));
        }
    }

    public class MathPotentialSpace : Group<SyntaxDiscardResult, MathLiteralResult>
    {
        public MathPotentialSpace() : base(new PotentialSpace()) { }
        public override MathLiteralResult Parse(CharacterStream stream)
        {
            return new MathLiteralResult(InnerToken.Parse(stream).Content);
        }
    }

    public class MathPotentialLine : Group<SyntaxDiscardResult, MathLiteralResult>
    {
        public MathPotentialLine() : base(new PotentialNewLine()) { }
        public override MathLiteralResult Parse(CharacterStream stream)
        {
            return new MathLiteralResult(InnerToken.Parse(stream).Content);
        }
    }
}