namespace math_parser.tokenizer
{
    public class Group<S> : Token<S> where S : ParseResult
    {
        private readonly IToken<S> inner_token;

        public Group(IToken<S> inner)
        {
            inner_token = inner;
        }

        public override (S, CharacterStream) Parse(CharacterStream stream) => inner_token.Parse(stream);

        public override bool CanParse(CharacterStream stream) => inner_token.CanParse(stream);

        public override CharacterStream PartialParse(CharacterStream stream) => inner_token.PartialParse(stream);

        public override bool CanPartialParse(CharacterStream stream) => inner_token.CanPartialParse(stream);
    }
}