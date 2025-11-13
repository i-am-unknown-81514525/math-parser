namespace math_parser.tokenizer
{
    public class AlphabeticChar : Group<SyntaxDiscardResult>
    {
        public AlphabeticChar() : base(
            new OrNoBacktrack<SyntaxDiscardResult>(
                new Literal("a"),
                new Literal("b"),
                new Literal("c"),
                new Literal("d"),
                new Literal("e"),
                new Literal("f"),
                new Literal("g"),
                new Literal("h"),
                new Literal("i"),
                new Literal("j"),
                new Literal("k"),
                new Literal("l"),
                new Literal("m"),
                new Literal("n"),
                new Literal("o"),
                new Literal("p"),
                new Literal("q"),
                new Literal("r"),
                new Literal("s"),
                new Literal("t"),
                new Literal("u"),
                new Literal("v"),
                new Literal("w"),
                new Literal("x"),
                new Literal("y"),
                new Literal("z")
            )
        ) {}
    }

    public class NumericalChar : Group<SyntaxDiscardResult>
    {
        public NumericalChar() : base(
            new OrNoBacktrack<SyntaxDiscardResult>(
                new Literal("0"),
                new Literal("1"),
                new Literal("2"),
                new Literal("3"),
                new Literal("4"),
                new Literal("5"),
                new Literal("6"),
                new Literal("7"),
                new Literal("8"),
                new Literal("9")
            )
        ) {}
    }

    public class VariableAtom : Group<TokenSequenceResult<ParseResult>, MathLiteralResult>
    {
        public VariableAtom() : base(
            new TokenSequence<ParseResult>(
                new AlphabeticChar(),
                new Repeat<SyntaxDiscardResult>(
                    new OrNoBacktrack<SyntaxDiscardResult>(
                        new AlphabeticChar(),
                        new NumericalChar()
                    ),
                    0,
                    Amount.Unbound
                ),
                new Maybe<TokenSequenceResult<ParseResult>>(
                    new TokenSequence<ParseResult>(
                        new Literal("_"),
                        new Repeat<SyntaxDiscardResult>(
                            new OrNoBacktrack<SyntaxDiscardResult>(
                                new AlphabeticChar(),
                                new NumericalChar()
                            ),
                            1,
                            Amount.Unbound
                        )
                    )
                )
            )
        )
        { }

        public override bool CanParse(CharacterStream stream)
        {
            if (Keyword.IsStartWithKeyword(stream.PeekAll()))
            {
                return false;
            }
            return base.CanParse(stream);
        }

        public override MathLiteralResult Parse(CharacterStream stream)
        {
            if (Keyword.IsStartWithKeyword(stream.PeekAll()))
            {
                throw new TokenParseBacktrackException($"Not valid path while attempting to parse keyword", stream.ptr, new System.Collections.Generic.List<string> {"!Keyword"}, stream.Peek(20));
            }
            CharacterStream cp = stream.Fork();
            inner_token.Parse(cp);
            string content = stream.TakeTo(cp);
            return new MathLiteralResult(content);
        }

        public override bool CanPartialParse(CharacterStream stream)
        {
            if (Keyword.IsStartWithKeyword(stream.PeekAll()))
            {
                return false;
            }
            return base.CanPartialParse(stream);
        }

        public override CharacterStream PartialParse(CharacterStream stream)
        {
            if (Keyword.IsStartWithKeyword(stream.PeekAll()))
            {
                throw new TokenParseBacktrackException($"Not valid path while attempting to parse keyword", stream.ptr, new System.Collections.Generic.List<string>() {"!Keyword"}, stream.Peek(20));
            }
            return base.PartialParse(stream);
        }
    }
}