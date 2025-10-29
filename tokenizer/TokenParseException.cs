using System;

namespace math_parser.tokenizer
{
    public class TokenParseException : Exception
    {
        public TokenParseException() : base() { }
        public TokenParseException(string reason) : base(reason) { }
    }

    public class TokenParseBacktrackException : TokenParseException
    {
        public TokenParseBacktrackException() : base() { }
        public TokenParseBacktrackException(string reason) : base(reason) { }
    }

    public class TokenParseNoBacktrackException : TokenParseException
    {
        public TokenParseNoBacktrackException() : base() { }
        public TokenParseNoBacktrackException(string reason) : base(reason) { }
    }
}