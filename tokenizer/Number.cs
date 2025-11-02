using System;
using System.Numerics;
using math_parser.math;

namespace math_parser.tokenizer
{

    public class NumberResult : MathAtomResult
    {
        public readonly BigInteger integerPart;
        public readonly BigInteger decimalMantissa;
        public readonly BigInteger decimalExponent;
        // ^ should be <= 0 and normalised (i.e. if it end with 0 in decimalMantissa, decimalExponent increase by one)
        // It would be like (integerPart + (10**decimalExponent)*("0."decimalMantissa))

        public NumberResult(BigInteger integerPart, BigInteger decimalMantissa, BigInteger decimalExponent)
        {
            if (decimalExponent > 0)
            {
                throw new InvalidOperationException("decimalExponent must be less than or equal to 0");
            }
            BigInteger extra = decimalMantissa / MathUtils.Pow(10, -decimalExponent);
            // Equivialent to decimalMantissa * MathUtils.Pow(10, decimalExponent), but since it operate on only integer, this order must be used
            integerPart += extra;
            decimalMantissa -= extra * MathUtils.Pow(10, -decimalExponent);
            if (decimalMantissa < 0)
            {
                decimalMantissa += MathUtils.Pow(10, -decimalExponent);
                integerPart--;
            }
            while (decimalMantissa % 10 == 0 && decimalMantissa != 0)
            {
                decimalMantissa /= 10;
                decimalExponent++; // Closer to 0, I am not doing something wrong since it is originally negative
            }
            this.integerPart = integerPart;
            this.decimalMantissa = decimalMantissa;
            this.decimalExponent = decimalExponent;
        }

        public Fraction AsFraction()
        {
            return new Fraction(integerPart * MathUtils.Pow(10, -decimalExponent) + decimalMantissa, MathUtils.Pow(10, -decimalExponent));
        }
    }

    public class Number : Group<ParseResult, NumberResult>
    {
        public Number() : base(
            new TokenSequence<ParseResult>(
                new OrNoBacktrack<ParseResult>(
                    new Literal("+"),
                    new Literal("-"),
                    new Literal("")
                ),
                new OrNoBacktrack<ParseResult>(
                    new Literal("1"),
                    new Literal("2"),
                    new Literal("3"),
                    new Literal("4"),
                    new Literal("5"),
                    new Literal("6"),
                    new Literal("7"),
                    new Literal("8"),
                    new Literal("9")
                ),
                new Repeat<ParseResult>(
                    new OrNoBacktrack<ParseResult>(
                        new Literal("0"),
                        new Literal("1"),
                        new Literal("2"),
                        new Literal("3"),
                        new Literal("4"),
                        new Literal("5"),
                        new Literal("6"),
                        new Literal("7"),
                        new Literal("8"),
                        new Literal("9")
                    ),
                    0,
                    Amount.Unbound
                ),
                new Maybe<ParseResult>(
                    new TokenSequence<ParseResult>(
                        new Literal("."),
                        new Repeat<ParseResult>(
                            new TokenSequence<ParseResult>(
                                new Repeat<ParseResult>(
                                    new OrNoBacktrack<ParseResult>(
                                        new Literal("1"),
                                        new Literal("2"),
                                        new Literal("3"),
                                        new Literal("4"),
                                        new Literal("5"),
                                        new Literal("6"),
                                        new Literal("7"),
                                        new Literal("8"),
                                        new Literal("9")
                                    ),
                                    0,
                                    Amount.Unbound
                                ),
                                new Repeat<ParseResult>(
                                    new Literal("0"),
                                    0,
                                    Amount.Unbound
                                )
                            ),
                            0,
                            Amount.Unbound
                        ),
                        new Repeat<ParseResult>(
                            new OrNoBacktrack<ParseResult>(
                                new Literal("1"),
                                new Literal("2"),
                                new Literal("3"),
                                new Literal("4"),
                                new Literal("5"),
                                new Literal("6"),
                                new Literal("7"),
                                new Literal("8"),
                                new Literal("9")
                            ),
                            0,
                            Amount.Unbound
                        )
                    )
                )
            )
        )
        {

        }

        public override NumberResult Parse(CharacterStream stream)
        {
            CharacterStream cp = stream.Clone();
            inner_token.Parse(cp);
            string content = stream.Take(cp.ptr - stream.ptr);
            if (content.Contains("."))
            {
                string[] all = content.Split('.');
                if (all.Length != 2)
                {
                    throw new InvalidOperationException("Number pasing err1");
                }
                string left = all[0];
                string right = all[1];
                int r_length = right.Length;
                BigInteger integerPart = BigInteger.Parse(left);
                BigInteger mantissa = BigInteger.Parse(right.TrimStart('0'));
                BigInteger exponent = new BigInteger(-r_length);
                return new NumberResult(integerPart, mantissa, exponent);
            }
            return new NumberResult(BigInteger.Parse(content), 0, 0);
        }
    }
}