using System.Collections.Generic;
using System.Linq;

namespace math_parser.tokenizer
{
    public class Or<S> : Token<S> where S : ParseResult
    {
        IToken<S>[] options;

        public Or(params IToken<S>[] options)
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

        public override S Parse(CharacterStream stream)
        {
            foreach (IToken<S> option in options)
            {
                CharacterStream inner = stream.Clone();
                try
                {
                    S v = option.Parse(inner);
                    return option.Parse(stream);
                }
                catch (TokenParseException)
                {

                }
            }
            throw new TokenParseBacktrackException("No valid path", stream.ptr, stream.Peek(20)); // i.e. we have backtrack all possible options
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