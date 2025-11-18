using System.Collections.Generic;
using System;
using System.Diagnostics;
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
                throw new ArgumentOutOfRangeException($"amount must be greater or equal to 0 (given: {amount}");
            }
            this.amount = (uint)(amount + 1);
        }

        public bool isUnbound => amount == 0;

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

    public class RepeatListResult<TS> : List<TS>, ParseResult where TS : ParseResult
    {
        public override string ToString() => ToString(0);
        public string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{ParseResultExtensions.Indent(indent)}RepeatListResult ({Count} items):");
            foreach (var result in this)
            {
                sb.Append(result.ToString(indent + 1));
            }
            return sb.ToString();
        }
    }

    public class Repeat<TS> : Token<RepeatListResult<TS>> where TS : ParseResult
    {

        IToken<TS> _token;
        readonly Amount _min;
        readonly Amount _max;

        public Repeat(IToken<TS> token, Amount min, Amount max)
        {
            _token = token;
            if (min.isUnbound)
            {
                min = new Amount(0);
            }
            if (!max.isUnbound && min.value > max.value)
            {
                throw new ArgumentOutOfRangeException($"min({min}) must be less than or equal to max({max})");
            }
            _min = min;
            _max = max;
        }

        public override bool CanParse(CharacterStream stream)
        {
            if (_min.value == 0)
            {
                return true;
            }
            try
            {
                Parse(stream.Clone());
                return true;
            }
            catch (TokenParseException) {
                return false;
            }
        }

        public override bool CanPartialParse(CharacterStream stream)
        {
            if (_min.value == 0)
            {
                return true;
            }
            return _token.CanPartialParse(stream);
        }

        public override RepeatListResult<TS> Parse(CharacterStream stream)
        {
            RepeatListResult<TS> results = new RepeatListResult<TS>();
            CharacterStream last = stream.Clone();
            CharacterStream curr = stream.Clone();
            for (uint i = 0; i < _min.value; i++)
            {
                TS v;
                v = _token.Parse(stream);
                results.Add(v);
            }
            last.JumpForwardTo(stream);
            curr.JumpForwardTo(stream);
            if (_max.isUnbound)
            {
                while (true)
                {
                    try
                    {
                        TS v;
                        v = _token.Parse(curr);
                        results.Add(v);
                        last.JumpForwardTo(curr);
                    }
                    catch (TokenParseException)
                    {
                        stream.JumpForwardTo(last);
                        return results;
                    }
                }
            }

            Debug.Assert(_min.value != null, "_min.value != null");
            for (uint i = (uint)_min.value; i < _max.value; i++)
            {
                try
                {
                    TS v;
                    v = _token.Parse(curr);
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
            if (_min.value == 0)
            {
                return stream;
            }
            return _token.PartialParse(stream);
        }
    }
}