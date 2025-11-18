using System.Collections.Generic;
using math_parser.math;

namespace math_parser.tokenizer
{
    public class Bracketed<TS> : Group<TokenSequenceResult<ParseResult>, TS> where TS : ParseResult
    {
        public Bracketed(IToken<TS> token) : base(
            new TokenSequence<ParseResult>(
                new Literal("("),
                (IToken<ParseResult>)token,
                new Literal(")")
            )
        )
        { }

        public override TS Parse(CharacterStream stream)
        {
            var result = InnerToken.Parse(stream);
            return (TS)result.ParseResult[1];
        }
    }
}