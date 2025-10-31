using System.Collections.Generic;
using math_parser.math;

namespace math_parser.tokenizer
{
    public class Bracketed<S> : Group<TokenSequenceResult<ParseResult>, S> where S : ParseResult
    {
        public Bracketed(IToken<S> token) : base(
            new TokenSequence<ParseResult>(
                new Literal("("),
                (IToken<ParseResult>)token,
                new Literal(")")
            )
        )
        { }

        public override S Parse(CharacterStream stream)
        {
            var result = inner_token.Parse(stream);
            return (S)result.parseResult[1];
        }
    }
}