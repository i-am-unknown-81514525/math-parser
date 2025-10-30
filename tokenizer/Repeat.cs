using System.Collections.Generic;
using System;

namespace math_parser.tokenizer
{
    public struct Amount
    {
        public uint amount { get; }

        public Amount(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount must be greater or equal to 0");
            }
            this.amount = (uint)(amount + 1);
        }

        public bool isUnbound
        {
            get => amount < 0;
            set { }
        }

        public static Amount Unbound = new Amount();

        public uint? value
        {
            get
            {
                if (amount == 0)
                {
                    return null;
                }
                return amount - 1;
            }
        }
    }

    public class ParseResultList<S> : List<S>, ParseResult where S : ParseResult {}

    public class Repeat<S> : Token<ParseResultList<S>> where S : ParseResult
    {

        IToken<S> token;
        Amount min;
        Amount max;

        public Repeat(IToken<S> token, Amount min, Amount max)
        {
            this.token = token;
            if (min.isUnbound)
            {
                min = new Amount(0);
            }
            if (!max.isUnbound && min.amount > max.amount)
            {
                throw new ArgumentOutOfRangeException("min must be less than or equal to max");
            }
            this.min = min;
            this.max = max;
        }

        public override bool CanParse(CharacterStream stream)
        {
            if (min.amount == 0)
            {
                return true;
            }
            try
            {
                this.Parse(stream.Clone());
                return true;
            }
            catch (TokenParseException) {
                return false;
            }
        }

        public override bool CanPartialParse(CharacterStream stream)
        {
            if (min.amount == 0)
            {
                return true;
            }
            return token.CanPartialParse(stream);
        }

        public override (ParseResultList<S>, CharacterStream) Parse(CharacterStream stream)
        {
            CharacterStream original = stream;
            ParseResultList<S> results = new ParseResultList<S>();
            CharacterStream last;
            for (uint i = 0; i < min.value; i++)
            {
                S v;
                (v, stream) = token.Parse(stream);
                results.Add(v);
            }
            if (max.isUnbound)
            {
                while (true)
                {
                    last = stream;
                    try
                    {
                        S v;
                        (v, stream) = token.Parse(stream);
                        results.Add(v);
                    }
                    catch (TokenParseException)
                    {
                        return (results, original.JumpForwardTo(last));
                    }
                }
                throw new InvalidOperationException("Wtf?");
            }
            for (uint i = (uint)min.value; i < max.value; i++)
            {
                last = stream.Clone();
                try
                {
                    S v;
                    (v, stream) = token.Parse(stream);
                    results.Add(v);
                }
                catch (TokenParseException)
                {
                    return (results, original.JumpForwardTo(last));
                }
            }
            return (results, original.JumpForwardTo(stream));
        }

        public override CharacterStream PartialParse(CharacterStream stream)
        {
            if (min.amount == 0)
            {
                return stream;
            }
            return token.PartialParse(stream);
        }
    }
}