using System.Collections.Generic;
using System.Linq;

namespace math_parser.tokenizer
{
    public class Or : Token
    {
        IBaseToken[] options;

        public Or(params IBaseToken[] options)
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

        public override (SyntaxDiscardResult, CharacterStream) Parse(CharacterStream stream)
        {
            foreach (IBaseToken option in options)
            {
                CharacterStream inner = stream.Clone();
                try
                {
                    option.Parse(inner);
                    return inner;
                }
                catch (TokenParseException)
                {

                }
            }
            throw new TokenParseBacktrackException("No valid path"); // i.e. we have backtrack all possible options
        }

        public override CharacterStream PartialParse(CharacterStream stream)
        {
            foreach (IBaseToken option in options)
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
            throw new TokenParseBacktrackException("No valid path");
        }
    }
}