using System.Collections.Generic;
using System.Linq;

namespace math_parser.tokenizer
{
    public class OrNoBacktrack<S> : Token<S> where S : ParseResult
    {
        IToken<S>[] options;

        public OrNoBacktrack(params IToken<S>[] options)
        {
            this.options = options.ToArray();
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

        public override S Parse(CharacterStream stream)
        {
            List<string> global_expected = new List<string>();
            for (int i = 0; i < options.Length; i++)
            {
                IToken<S> token = options[i];
                if (token.CanPartialParse(stream))
                {
                    CharacterStream inner = stream.Fork();
                    try
                    {
                        S result = token.Parse(inner);
                        stream.JumpForwardTo(inner);
                        return result;
                    }
                    catch (TokenParseException e)
                    {
                        List<string> local = e.GetExpectedTokens().ToList();
                        global_expected.AddRange(local);
                        throw new TokenParseNoBacktrackException(
                            $"No valid path, Attempt path {i} which successfully partial parse but failed to be parsed entirely", 
                            stream.ptr, 
                            global_expected, 
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
                        global_expected.AddRange(e.GetExpectedTokens());
                    }
                }
            }
            throw new TokenParseBacktrackException("No valid path", stream.ptr, global_expected, stream.Peek(20)); // This is using backtrack instead of no backtrack as the essential identical path
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
            foreach (IToken<S> option in options)
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