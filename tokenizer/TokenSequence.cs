using System.Collections.Generic;
using System.Linq;

namespace math_parser.tokenizer
{
    public class GroupResult<S> : ParseResult where S : ParseResult
    {
        public S[] parseResult;

        public GroupResult(IEnumerable<S> r)
        {
            parseResult = r.ToArray();
        }
    }

    public class TokenSequence<S> : Token<GroupResult<S>>, IEnumerable<IToken<S>> where S : ParseResult
    {
        private List<IToken<S>> tokens = new List<IToken<S>>();
        private bool _writable = true;
        private IToken<S>[] immutable_tokens;

        public TokenSequence()
        {

        }
        
        public TokenSequence(params IToken<S>[] tokens)
        {
            immutable_tokens = (IToken<S>[])tokens.Clone();
            _writable = false;
        }

        public void Add(IToken<S> token)
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

        public IToken<S> this[int idx]
        {
            get => immutable_tokens[idx];
        }

        public IEnumerator<IToken<S>> GetEnumerator()
        {
            MakeImmutable();
            return ((IEnumerable<IToken<S>>)immutable_tokens).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator TokenSequence<S>(List<IToken<S>> tokens)
        {
            return new TokenSequence<S>(tokens.ToArray());
        }

        public static implicit operator TokenSequence<S>(IToken<S>[] tokens)
        {
            return new TokenSequence<S>(tokens.ToArray());
        }

        public override GroupResult<S> Parse(CharacterStream stream)
        {
            MakeImmutable();
            List<S> r = new List<S>();
            foreach (IToken<S> token in immutable_tokens)
            {
                r.Add(token.Parse(stream));
            }
            return new GroupResult<S>(r);
        }

        public override bool CanParse(CharacterStream stream)
        {
            MakeImmutable();
            CharacterStream clone = stream.Fork();
            foreach (IToken token in immutable_tokens)
            {
                if (!token.CanParse(clone)) return false;
            }
            return true;
        }

        public override CharacterStream PartialParse(CharacterStream stream)
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
        public override bool CanPartialParse(CharacterStream stream)
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