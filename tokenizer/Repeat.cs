using System.Collections.Generic;
using System;
using System.Text;

namespace math_parser.tokenizer
{
    public struct Amount
    {
        private uint amount { get; }

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
            get => amount == 0;
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


        public static implicit operator Amount(int v) => new Amount(v);
    }

    public class RepeatListResult<S> : List<S>, ParseResult where S : ParseResult
    {
        public override string ToString() => ToString(0);
        public string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{ParseResultExtensions.Indent(indent)}RepeatListResult ({this.Count} items):");
            foreach (var result in this)
            {
                sb.Append(result.ToString(indent + 1));
            }
            return sb.ToString();
        }
    }

    public class Repeat<S> : Token<RepeatListResult<S>> where S : ParseResult
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
            if (!max.isUnbound && min.value > max.value)
            {
                throw new ArgumentOutOfRangeException("min must be less than or equal to max");
            }
            this.min = min;
            this.max = max;
        }

        public override bool CanParse(CharacterStream stream)
        {
            if (min.value == 0)
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
            if (min.value == 0)
            {
                return true;
            }
            return token.CanPartialParse(stream);
        }

        public override RepeatListResult<S> Parse(CharacterStream stream)
        {
            RepeatListResult<S> results = new RepeatListResult<S>();
            CharacterStream last = stream.Clone();
            CharacterStream curr = stream.Clone();
            for (uint i = 0; i < min.value; i++)
            {
                S v;
                v = token.Parse(stream);
                results.Add(v);
            }
            last.JumpForwardTo(stream);
            curr.JumpForwardTo(stream);
            if (max.isUnbound)
            {
                while (true)
                {
                    try
                    {
                        S v;
                        v = token.Parse(curr);
                        results.Add(v);
                        last.JumpForwardTo(curr);
                    }
                    catch (TokenParseException)
                    {
                        stream.JumpForwardTo(last);
                        return results;
                    }
                }
                throw new InvalidOperationException("Wtf?");
            }
            for (uint i = (uint)min.value; i < max.value; i++)
            {
                try
                {
                    S v;
                    v = token.Parse(curr);
                    results.Add(v);
                    last.JumpForwardTo(curr);
                }
                catch (TokenParseException)
                {
                    stream.JumpForwardTo(last);
                    return results;
                }
            }
            stream.JumpForwardTo(last);
            return results;
        }

        public override CharacterStream PartialParse(CharacterStream stream)
        {
            if (min.value == 0)
            {
                return stream;
            }
            return token.PartialParse(stream);
        }
    }
}