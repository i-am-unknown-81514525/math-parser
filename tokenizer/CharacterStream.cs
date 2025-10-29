using math_parser.utils;

namespace math_parser.tokenizer
{
    public class CharacterStream
    {
        SharedImmutableString _base;
        int ptr = 0;

        public CharacterStream(SharedImmutableString str)
        {
            this._base = str;
        }

        public char Peek()
        {
            return _base[ptr];
        }

        public void Advance()
        {
            ptr++;
        }

        public char Take()
        {
            char v = Peek();
            Advance();
            return v;
        }

        public string Peek(int amount)
        {
            return this._base.SubString(ptr, amount);
        }
        
        public void Advance(int amount)
        {
            ptr += amount;
        }

        public string Take(int amount)
        {
            string v = Peek(amount);
            Advance(amount);
            return v;
        }
    }
}