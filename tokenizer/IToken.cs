using System;

namespace math_parser.tokenizer
{
    public interface IToken : IToken<SyntaxDiscardResult>
    {
    }

    public interface IToken<T> : IBaseToken where T : ParseResult
    {
        (T curr, CharacterStream other) Parse(CharacterStream stream);
    }

    public interface IBaseToken
    {
        (ParseResult curr, CharacterStream other) Parse(CharacterStream stream);
        bool CanParse(CharacterStream stream);

        CharacterStream PartialParse(CharacterStream stream);
        bool CanPartialParse(CharacterStream stream);
    }

    public abstract class Token : IToken
    {
        (ParseResult, CharacterStream) IBaseToken.Parse(CharacterStream stream)
        {
            return Parse(stream);
        }

        public abstract (SyntaxDiscardResult, CharacterStream) Parse(CharacterStream stream);
        public abstract bool CanParse(CharacterStream stream);
        public abstract CharacterStream PartialParse(CharacterStream stream);
        public abstract bool CanPartialParse(CharacterStream stream);
    }
    
    public abstract class Token<S> : IToken<S> where S : ParseResult {
        (ParseResult, CharacterStream) IBaseToken.Parse(CharacterStream stream)
        {
            return Parse(stream);
        }

        public abstract (S, CharacterStream) Parse(CharacterStream stream);
        public abstract bool CanParse(CharacterStream stream);
        public abstract CharacterStream PartialParse(CharacterStream stream);
        public abstract bool CanPartialParse(CharacterStream stream);
    }
}