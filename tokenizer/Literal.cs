using System;
using System.Collections.Generic;

namespace math_parser.tokenizer
{
    public class Literal : Literal<SyntaxDiscardResult>
    {
        public Literal(string value) : base(value)
        {
        }
    }

    public class Literal<TS> : Token<TS> where TS : ParseResult
    {
        public readonly string Content;

        public Literal(string value)
        {
            if (value is null) throw new NullReferenceException("Literal cannot be null");
            Content = value;
        }

        public virtual TS Constructor(string content)
        {
            if (!(new SyntaxDiscardResult(content) is TS v))
            {
                throw new NullReferenceException("This is not SyntaxDiscardResult, you must override the constructor");
            }
            return v;
        }

        public override bool CanParse(CharacterStream stream)
        {
            if (Content.Length == 0) return true;
            return stream.Peek(Content.Length) == Content;
        }

        public override bool CanPartialParse(CharacterStream stream)
        {
            if (Content.Length == 0) return true;
            return stream.Peek() == Content[0];
        }

        public override TS Parse(CharacterStream stream)
        {
            if (Content.Length == 0) return Constructor(Content);
            if (stream.Peek(Content.Length) == Content)
            {
                stream.Take(Content.Length);
                return Constructor(Content);
            }
            throw new TokenParseBacktrackException($"Not valid path, expected literal {Content}", stream.ptr, new List<string> {$"\"{Content}\""},stream.Peek(20));
        }

        public override CharacterStream PartialParse(CharacterStream stream)
        {
            if (Content.Length == 0) return stream;
            if (stream.Take() == Content[0])
            {
                return stream;
            }
            throw new TokenParseBacktrackException($"Not valid path, expected literal {Content}", stream.ptr, new List<string> {$"\"{Content[0]}\""},stream.Peek(20));
        }
    }
}