using System;

namespace math_parser.tokenizer
{
    public interface IToken : IToken<SyntaxDiscardResult>
    {
    }

    public interface IToken<out T> : IBaseToken where T : ParseResult
    {
        new T Parse(CharacterStream stream);
    }

    public interface IBaseToken
    {
        (ParseResult curr, CharacterStream other) Parse(CharacterStream stream);
        bool CanParse(CharacterStream stream);
        // This should not mutated the character stream in any way, only operate on clone of the character stream

        CharacterStream PartialParse(CharacterStream stream);
        // Design: There are no guarentee of what character stream is returned from this function, or the action taken on the input stream

        bool CanPartialParse(CharacterStream stream);
        // This should not mutated the character stream in any way, only operate on clone of the character stream
    }

    public abstract class Token : IToken
    {
        (ParseResult, CharacterStream) IBaseToken.Parse(CharacterStream stream)
        {
            return (Parse(stream), stream);
        }

        public abstract SyntaxDiscardResult Parse(CharacterStream stream);
        public abstract bool CanParse(CharacterStream stream);
        public abstract CharacterStream PartialParse(CharacterStream stream);
        public abstract bool CanPartialParse(CharacterStream stream);
    }
    
    public abstract class Token<S> : IToken<S> where S : ParseResult {
        (ParseResult, CharacterStream) IBaseToken.Parse(CharacterStream stream)
        {
            return (Parse(stream), stream);
        }

        public abstract S Parse(CharacterStream stream);
        public abstract bool CanParse(CharacterStream stream);
        public abstract CharacterStream PartialParse(CharacterStream stream);
        public abstract bool CanPartialParse(CharacterStream stream);
    }
}