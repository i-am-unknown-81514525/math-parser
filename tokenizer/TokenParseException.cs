using System;
using System.Collections.Generic;
using System.Linq;

namespace math_parser.tokenizer
{
    public class TokenParseException : Exception
    {
        public readonly string Reason;
        public readonly int Ptr;
        public readonly string Next;
        private readonly List<string> _expectedTokens;
        public TokenParseException(
            string reason, 
            int ptr, 
            List<string> expected, 
            string next = "N/A"
        ) : base($"{reason} at pointer {ptr}, Expected [{string.Join(", ", expected).Replace("\n", "\\n").Replace("\r", "\\r")}], Procedding stream: {next.Replace("\n", "\\n").Replace("\r", "\\r")}")
        {
            _expectedTokens = expected.ToList();
            Reason = reason;
            Ptr = ptr;
            Next = next;
        }

        public string[] GetExpectedTokens()
        {
            return _expectedTokens.ToArray();
        }

        public virtual TokenParseException AddExpectedTokens(params string[] tokens)
        {
            List<string> expectedTokens = _expectedTokens.ToList();
            expectedTokens.AddRange(tokens);
            return new TokenParseException(
                Reason, 
                Ptr, 
                expectedTokens.ToList(),
                Next
            );
        }
    }

    public class TokenParseBacktrackException : TokenParseException
    {
        public TokenParseBacktrackException(string reason, int ptr, List<string> expected, string next = "N/A") : base(reason, ptr, expected, next) { }

        public override TokenParseException AddExpectedTokens(params string[] tokens)
        {
            List<string> expectedTokens = GetExpectedTokens().ToList();
            expectedTokens.AddRange(tokens);
            return new TokenParseBacktrackException(
                Reason, 
                Ptr, 
                expectedTokens.ToList(),
                Next
            );
        }
    }

    public class TokenParseNoBacktrackException : TokenParseException
    {
        public TokenParseNoBacktrackException(
            string reason, 
            int ptr, 
            List<string> expected, 
            string next = "N/A"
        ) : base(reason, ptr, expected, next) { }

        public override TokenParseException AddExpectedTokens(params string[] tokens)
        {
            List<string> expectedTokens = GetExpectedTokens().ToList();
            expectedTokens.AddRange(tokens);
            return new TokenParseNoBacktrackException(
                Reason, 
                Ptr, 
                expectedTokens.ToList(),
                Next
            );
        }
    }
}