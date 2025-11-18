namespace math_parser.tokenizer
{
    public class Group<TS> : Token<TS> where TS : ParseResult
    {
        protected readonly IToken<TS> InnerToken;

        public Group(IToken<TS> inner)
        {
            InnerToken = inner;
        }

        public override TS Parse(CharacterStream stream) => InnerToken.Parse(stream);

        public override bool CanParse(CharacterStream stream) => InnerToken.CanParse(stream);

        public override CharacterStream PartialParse(CharacterStream stream) => InnerToken.PartialParse(stream);

        public override bool CanPartialParse(CharacterStream stream) => InnerToken.CanPartialParse(stream);
    }

    public abstract class Group<TS, T> : Token<T> where TS : ParseResult where T : ParseResult
    {
        protected readonly IToken<TS> InnerToken;

        public Group(IToken<TS> inner)
        {
            InnerToken = inner;
        }

        public override bool CanParse(CharacterStream stream) => InnerToken.CanParse(stream);

        public override CharacterStream PartialParse(CharacterStream stream) => InnerToken.PartialParse(stream);

        public override bool CanPartialParse(CharacterStream stream) => InnerToken.CanPartialParse(stream);
    }
}