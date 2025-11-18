using System;
using math_parser.utils;

namespace math_parser.tokenizer
{
    public class CharacterStream : System.IEquatable<CharacterStream>
    {
        public readonly SharedImmutableString Base;
        public int ptr { get; internal set; } = 0;

        public CharacterStream(SharedImmutableString str)
        {
            this.Base = str;
        }

        public CharacterStream(SharedImmutableString str, int ptr)
        {
            this.Base = str;
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
            return this.Base.SubString(ptr, amount);
        }

        public string PeekAll()
        {
            return this.Base.SubString(ptr);
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
            return left.ptr == right.ptr && object.ReferenceEquals(left.Base, right.Base); // fastcheck, require copy to use the same shared immutable string
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
            if (o as CharacterStream is null) return false;
            return this == (o as CharacterStream);
        }

        public override int GetHashCode()
        {
            return Base.GetHashCode() ^ ptr.GetHashCode();
        }

        public CharacterStream JumpForwardTo(CharacterStream src)
        {
            if (!object.ReferenceEquals(src.Base, this.Base))
            {
                throw new InvalidOperationException("Not the same string");
            }
            if (this.ptr > src.ptr)
            {
                throw new InvalidOperationException("You cannot jump backward");
            }
            this.ptr = src.ptr;
            return this;
        }

        public string TakeTo(CharacterStream src)
        {
            if (!object.ReferenceEquals(src.Base, this.Base))
            {
                throw new InvalidOperationException("Not the same string");
            }
            if (this.ptr > src.ptr)
            {
                throw new InvalidOperationException("You cannot jump backward");
            }
            return Take(src.ptr - ptr);
        }
        
        public bool isEof
        {
            get => ptr + 1 >= Base.length;
        }

        // public static implicit operator (SyntaxDiscardResult, CharacterStream)(CharacterStream stream) => (SyntaxDiscardResult.Empty, stream);
    }
}