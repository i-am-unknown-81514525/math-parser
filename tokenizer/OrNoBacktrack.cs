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
            for (int i = 0; i < options.Length; i++)
            {
                IToken<S> token = options[i];
                if (token.CanPartialParse(stream))
                {
                    CharacterStream inner = stream.Fork();
                    try
                    {
                        S v = token.Parse(inner);
                        return token.Parse(stream);
                    }
                    catch (TokenParseException)
                    {
                        throw new TokenParseNoBacktrackException($"No valid path, Attempt path {i} which successfully partial parse but failed to be parsed entirely", stream.ptr, stream.Peek(20));
                    }
                }
            }
            throw new TokenParseBacktrackException("No valid path", stream.ptr, stream.Peek(20)); // This is using backtrack instead of no backtrack as the essential identical path
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
            foreach (IToken<S> option in options)
            {
                CharacterStream inner = stream.Clone();
                try
                {
                    option.PartialParse(inner);
                    return inner;
                }
                catch (TokenParseException)
                {

                }
            }
            throw new TokenParseBacktrackException("No valid path", stream.ptr, stream.Peek(20));
        }
    }
}