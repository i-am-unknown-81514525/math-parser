using System;

namespace math_parser.tokenizer
{
    public class TokenParseException : Exception
    {
        public TokenParseException() : base() { }
        public TokenParseException(string reason, int ptr, string next = "N/A") : base($"{reason} at pointer {ptr}, Procedding stream: {next}") { }
    }

    public class TokenParseBacktrackException : TokenParseException
    {
        public TokenParseBacktrackException() : base() { }
        public TokenParseBacktrackException(string reason, int ptr, string next = "N/A") : base(reason, ptr, next) { }
    }

    public class TokenParseNoBacktrackException : TokenParseException
    {
        public TokenParseNoBacktrackException() : base() { }
        public TokenParseNoBacktrackException(string reason, int ptr, string next = "N/A") : base(reason, ptr, next) { }
    }
}