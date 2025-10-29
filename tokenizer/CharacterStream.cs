using math_parser.utils;

namespace math_parser.tokenizer
{
    public class CharacterStream : IEquatable<CharacterStream>
    {
        public readonly SharedImmutableString _base;
        public readonly int ptr = 0;

        public CharacterStream(SharedImmutableString str)
        {
            this._base = str;
        }

        public CharacterStream(SharedImmutableString str, int ptr)
        {
            this._base = str;
            ptr = ptr;
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

        public static bool operator ==(CharacterStream left, CharacterStream right)
        {
            if (left is null)
            {
                return right is null;
            }
            return left.ptr == right.ptr && object.ReferenceEquals(left._base, right._base); // fastcheck, require copy to use the same shared immutable string
        }

        public static bool operator !=(CharacterStream left, CharacterStream right) => !(left == right);

        public CharacterStream Fork()
        {
            return new CharacterStream(_base, ptr);
        }

        public CharacterStream Clone()
        {
            return new CharacterStream(_base, ptr);
        }
    }
}