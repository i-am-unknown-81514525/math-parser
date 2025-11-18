using System.Collections.Generic;
using System.Linq;

namespace math_parser.tokenizer
{
    public class Or<TS> : Token<TS> where TS : ParseResult
    {
        IToken<TS>[] _options;

        public Or(params IToken<TS>[] options)
        {
            this._options = options.ToArray();
        }

        public override bool CanParse(CharacterStream stream)
        {
            try
            {
                this.Parse(stream.Clone());
                return true;
            }
            catch (TokenParseException)
            {
                return false;
            }
        }

        public override bool CanPartialParse(CharacterStream stream)
        {
            try
            {
                this.PartialParse(stream.Clone());
                return true;
            }
            catch (TokenParseException)
            {
                return false;
            }
        }

        public override TS Parse(CharacterStream stream)
        {
            List<string> expected = new List<string>();
            foreach (IToken<TS> option in _options)
            {
                CharacterStream inner = stream.Clone();
                try
                {
                    TS v = option.Parse(inner);
                    stream.JumpForwardTo(inner);
                    return v;
                }
                catch (TokenParseException e)
                {
                    expected.AddRange(e.GetExpectedTokens());
                }
            }
            throw new TokenParseBacktrackException("No valid path", stream.ptr, expected, stream.Peek(20)); // i.e. we have backtrack all possible options
        }

        public override CharacterStream PartialParse(CharacterStream stream)
        {
            List<string> expected = new List<string>();
            foreach (IToken<TS> option in _options)
            {
                CharacterStream inner = stream.Clone();
                try
                {
                    option.PartialParse(inner);
                    return inner;
                }
                catch (TokenParseException e)
                {
                    expected.AddRange(e.GetExpectedTokens());
                }
            }
            throw new TokenParseBacktrackException("No valid path", stream.ptr, expected, stream.Peek(20));
        }
    }
}