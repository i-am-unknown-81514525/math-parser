using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace math_parser.tokenizer
{
    public class TokenSequenceResult<TS> : ParseResult where TS : ParseResult
    {
        public TS[] ParseResult;

        public TokenSequenceResult(IEnumerable<TS> r)
        {
            ParseResult = r.ToArray();
        }

        public override string ToString() => ToString(0);
        public string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{ParseResultExtensions.Indent(indent)}TokenSequenceResult:");
            foreach (var result in ParseResult)
            {
                sb.Append(result.ToString(indent + 1));
            }
            return sb.ToString();
        }
    }

    public class TokenSequence<TS> : Token<TokenSequenceResult<TS>>, IEnumerable<IToken<TS>> where TS : ParseResult
    {
        private List<IToken<TS>> _tokens = new List<IToken<TS>>();
        private bool _writable = true;
        private IToken<TS>[] _immutableTokens;

        public TokenSequence()
        {

        }
        
        public TokenSequence(params IToken<TS>[] tokens)
        {
            _immutableTokens = (IToken<TS>[])tokens.Clone();
            _writable = false;
        }

        public void Add(IToken<TS> token)
        {
            _tokens.Add(token);
        }

        public void MakeImmutable()
        {
            if (!_writable) return;
            _writable = false;
            _immutableTokens = _tokens.ToArray();
            _tokens = null;
        }

        public IToken<TS> this[int idx]
        {
            get => _immutableTokens[idx];
        }

        public IEnumerator<IToken<TS>> GetEnumerator()
        {
            MakeImmutable();
            return ((IEnumerable<IToken<TS>>)_immutableTokens).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator TokenSequence<TS>(List<IToken<TS>> tokens)
        {
            return new TokenSequence<TS>(tokens.ToArray());
        }

        public static implicit operator TokenSequence<TS>(IToken<TS>[] tokens)
        {
            return new TokenSequence<TS>(tokens.ToArray());
        }

        public override TokenSequenceResult<TS> Parse(CharacterStream stream)
        {
            MakeImmutable();
            List<TS> r = new List<TS>();
            foreach (IToken<TS> token in _immutableTokens)
            {
                r.Add(token.Parse(stream));
            }
            return new TokenSequenceResult<TS>(r);
        }

        public override bool CanParse(CharacterStream stream)
        {
            MakeImmutable();
            CharacterStream clone = stream.Fork();
            foreach (IToken<TS> token in _immutableTokens)
            {
                if (!token.CanParse(clone)) return false;
            }
            return true;
        }

        public override CharacterStream PartialParse(CharacterStream stream)
        {
            MakeImmutable();
            CharacterStream ori = stream.Fork();
            foreach (IToken<TS> token in _immutableTokens)
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
            foreach (IToken<TS> token in _immutableTokens)
            {   try
                {
                    token.PartialParse(curr);
                    if (curr != ori)
                    {
                        break;
                    }
                }
                catch (TokenParseException) {
                    return false;
                }
            }
            return true;
        }
    }
}