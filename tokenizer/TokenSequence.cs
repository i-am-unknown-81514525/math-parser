using System.Collections.Generic;
using System.Linq;

namespace math_parser.tokenizer
{
    public class TokenSequence : IEnumerable<IToken>, IToken
    {
        private List<IToken> tokens = new List<IToken>();
        private bool _writable = true;
        private IToken[] immutable_tokens;

        public TokenSequence()
        {

        }
        
        public TokenSequence(params IToken[] tokens)
        {
            immutable_tokens = (IToken[])tokens.Clone();
            _writable = false;
        }

        public void Add(IToken token)
        {
            tokens.Add(token);
        }

        public void MakeImmutable()
        {
            if (!_writable) return;
            _writable = false;
            immutable_tokens = tokens.ToArray();
            tokens = null;
        }

        public IToken this[int idx]
        {
            get => immutable_tokens[idx];
        }

        public IEnumerator<IToken> GetEnumerator()
        {
            MakeImmutable();
            return ((IEnumerable<IToken>)immutable_tokens).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator TokenSequence(List<IToken> tokens)
        {
            return new TokenSequence(tokens.ToArray());
        }

        public static implicit operator TokenSequence(IToken[] tokens)
        {
            return new TokenSequence(tokens.ToArray());
        }

        public CharacterStream Parse(CharacterStream stream)
        {
            MakeImmutable();
            foreach (IToken token in immutable_tokens)
            {
                token.Parse(stream);
            }
        }
        public bool CanParse(CharacterStream stream)
        {
            MakeImmutable();
            CharacterStream clone = stream.Fork();
            foreach (IToken token in immutable_tokens)
            {
                token.Parse(clone);
            }
        }

        public CharacterStream PartialParse(CharacterStream stream)
        {
            MakeImmutable();
            CharacterStream ori = stream.Fork();
            foreach (IToken token in immutable_tokens)
            {
                token.PartialParse(stream);
                if (stream != ori)
                {
                    break;
                }
            }
            return stream;
        }
        public bool CanPartialParse(CharacterStream stream)
        {
            MakeImmutable();
            CharacterStream ori = stream.Fork();
            CharacterStream curr = stream.Fork();
            foreach (IToken token in immutable_tokens)
            {
                token.PartialParse(curr);
                if (curr != ori)
                {
                    break;
                }
            }
            return true;
        }
    }
}