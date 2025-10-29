using System;

namespace math_parser.tokenizer
{
    public interface IToken : IToken<SyntaxDiscardResult>
    {
    }

    public interface IToken<T> where T : ParseResult
    {
        (T curr, CharacterStream other) Parse(CharacterStream stream);
        bool CanParse(CharacterStream stream);

        CharacterStream PartialParse(CharacterStream stream);
        bool CanPartialParse(CharacterStream stream);
    }
}