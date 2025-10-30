using System;

namespace math_parser.tokenizer
{
    public class Literal : Literal<SyntaxDiscardResult>
    {
        public Literal(string value) : base(value)
        {
        }
    }

    public class Literal<S> : Token<S> where S : ParseResult
    {
        private string content;

        public Literal(string value)
        {
            if (value is null) throw new NullReferenceException("Literal cannot be null");
            this.content = value;
        }

        public virtual S Constructor(string content)
        {
            S v = new SyntaxDiscardResult(content) as S;
            if (v is null)
            {
                throw new NullReferenceException("This is not SyntaxDiscardResult, you must override the constructor");
            }
            return v;
        }

        public override bool CanParse(CharacterStream stream)
        {
            if (content.Length == 0) return true;
            return stream.Peek(content.Length) == content;
        }

        public override bool CanPartialParse(CharacterStream stream)
        {
            if (content.Length == 0) return true;
            return stream.Peek() == content[0];
        }

        public override (S, CharacterStream) Parse(CharacterStream stream)
        {
            if (content.Length == 0) return (Constructor(content), stream);
            if (stream.Take(content.Length) == content)
            {
                return (Constructor(content), stream);
            }
            throw new TokenParseBacktrackException("Not valid path");
        }

        public override CharacterStream PartialParse(CharacterStream stream)
        {
            if (content.Length == 0) return stream;
            if (stream.Take() == content[0])
            {
                return stream;
            }
            throw new TokenParseBacktrackException("Not valid path");
        }
    }
}