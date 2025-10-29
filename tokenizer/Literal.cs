using System;

namespace math_parser.tokenizer
{
    public class Literal : IToken
    {
        private string content;

        public Literal(string value)
        {
            if (value is null) throw new NullReferenceException("Literal cannot be null");
            this.content = value;
        }

        public bool CanParse(CharacterStream stream)
        {
            if (content.Length == 0) return true;
            return stream.Peek(content.Length) == content;
        }

        public bool CanPartialParse(CharacterStream stream)
        {
            if (content.Length == 0) return true;
            return stream.Peek() == content[0];
        }

        public (SyntaxDiscardResult, CharacterStream) Parse(CharacterStream stream)
        {
            if (content.Length == 0) return stream;
            if (stream.Take(content.Length) == content)
            {
                return stream;
            }
            throw new TokenParseBacktrackException("Not valid path");
        }

        public CharacterStream PartialParse(CharacterStream stream)
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