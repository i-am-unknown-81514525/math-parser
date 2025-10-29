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

        public CharacterStream Parse(CharacterStream stream)
        {
            throw new NotImplementedException();
        }

        public CharacterStream PartialParse(CharacterStream stream)
        {
            throw new NotImplementedException();
        }
    }
}