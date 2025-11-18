using System.Collections.Generic;
using System.Linq;

namespace math_parser.tokenizer
{
    public class OrNoBacktrack<TS> : Token<TS> where TS : ParseResult
    {
        IToken<TS>[] _options;

        public OrNoBacktrack(params IToken<TS>[] options)
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

        public override TS Parse(CharacterStream stream)
        {
            List<string> globalExpected = new List<string>();
            for (int i = 0; i < _options.Length; i++)
            {
                IToken<TS> token = _options[i];
                if (token.CanPartialParse(stream))
                {
                    CharacterStream inner = stream.Fork();
                    try
                    {
                        TS result = token.Parse(inner);
                        stream.JumpForwardTo(inner);
                        return result;
                    }
                    catch (TokenParseException e)
                    {
                        List<string> local = e.GetExpectedTokens().ToList();
                        globalExpected.AddRange(local);
                        throw new TokenParseNoBacktrackException(
                            $"No valid path, Attempt path {i} which successfully partial parse but failed to be parsed entirely", 
                            stream.ptr, 
                            globalExpected, 
                            stream.Peek(20)
                        );
                    }
                } else
                {
                    try
                    {
                        token.PartialParse(stream.Clone());
                    }
                    catch (TokenParseException e)
                    {
                        globalExpected.AddRange(e.GetExpectedTokens());
                    }
                }
            }
            throw new TokenParseBacktrackException("No valid path", stream.ptr, globalExpected, stream.Peek(20)); // This is using backtrack instead of no backtrack as the essential identical path
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