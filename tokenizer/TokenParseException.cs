using System;
using System.Collections.Generic;
using System.Linq;

namespace math_parser.tokenizer
{
    public class TokenParseException : Exception
    {
        public readonly string reason;
        public readonly int ptr;
        public readonly string next;
        private List<string> expectedTokens = new List<string>();
        public TokenParseException(
            string reason, 
            int ptr, 
            List<string> expected, 
            string next = "N/A"
        ) : base($"{reason} at pointer {ptr}, Expected {string.Join(", ", expected)}, Procedding stream: {next}")
        {
            this.expectedTokens = expected.ToList();
            this.reason = reason;
            this.ptr = ptr;
            this.next = next;
        }

        public string[] GetExpectedTokens()
        {
            return expectedTokens.ToArray();
        }

        public virtual TokenParseException AddExpectedTokens(params string[] tokens)
        {
            List<string> expectedTokens = this.expectedTokens.ToList();
            expectedTokens.AddRange(tokens);
            return new TokenParseException(
                reason, 
                ptr, 
                expectedTokens.ToList(),
                next
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
                reason, 
                ptr, 
                expectedTokens.ToList(),
                next
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
                reason, 
                ptr, 
                expectedTokens.ToList(),
                next
            );
        }
    }
}