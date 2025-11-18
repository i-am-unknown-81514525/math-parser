using System;
using math_parser.utils;

namespace math_parser.tokenizer
{
    public class CharacterStream : IEquatable<CharacterStream>
    {
        public readonly SharedImmutableString Base;
        public int ptr { get; internal set; }

        public CharacterStream(SharedImmutableString str)
        {
            Base = str;
        }

        public CharacterStream(SharedImmutableString str, int ptr)
        {
            Base = str;
            this.ptr = ptr;
        }

        public char Peek()
        {
            return Base[ptr];
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
            if (amount == 0) return "";
            return Base.SubString(ptr, amount);
        }

        public string PeekAll()
        {
            return Base.SubString(ptr);
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

            if (right is null) return false;
            return left.ptr == right.ptr && ReferenceEquals(left.Base, right.Base); // fastcheck, require copy to use the same shared immutable string
        }

        public static bool operator !=(CharacterStream left, CharacterStream right) => !(left == right);

        public CharacterStream Fork()
        {
            return new CharacterStream(Base, ptr);
        }

        public CharacterStream Clone()
        {
            return new CharacterStream(Base, ptr);
        }

        public bool Equals(CharacterStream other)
        {
            return this == other;
        }

        public override bool Equals(object o)
        {
            if (o is null && this is null) return true;
            if (!(o is CharacterStream stream)) return false;
            return this == stream;
        }

        public override int GetHashCode()
        {
            return Base.GetHashCode();
        }

        public CharacterStream JumpForwardTo(CharacterStream src)
        {
            if (!ReferenceEquals(src.Base, Base))
            {
                throw new InvalidOperationException("Not the same string");
            }
            if (ptr > src.ptr)
            {
                throw new InvalidOperationException("You cannot jump backward");
            }
            ptr = src.ptr;
            return this;
        }

        public string TakeTo(CharacterStream src)
        {
            if (!ReferenceEquals(src.Base, Base))
            {
                throw new InvalidOperationException("Not the same string");
            }
            if (ptr > src.ptr)
            {
                throw new InvalidOperationException("You cannot jump backward");
            }
            return Take(src.ptr - ptr);
        }
        
        public bool isEof => ptr + 1 >= Base.Length;

        // public static implicit operator (SyntaxDiscardResult, CharacterStream)(CharacterStream stream) => (SyntaxDiscardResult.Empty, stream);
    }
}