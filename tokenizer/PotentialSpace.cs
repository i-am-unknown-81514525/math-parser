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
            inner_token.Parse(cp);
            return new SyntaxDiscardResult(stream.TakeTo(cp));
        }
    }

    public class PotentialNewLine : Group<ParseResult, SyntaxDiscardResult>
    {
        public PotentialNewLine() : base(
            new TokenSequence<ParseResult>(
                new PotentialSpace(),
                new Literal("\n"),
                new PotentialSpace()
            )
        )
        { }

        public override SyntaxDiscardResult Parse(CharacterStream stream)
        {
            CharacterStream cp = stream.Fork();
            inner_token.Parse(cp);
            return new SyntaxDiscardResult(stream.TakeTo(cp));
        }
    }
}