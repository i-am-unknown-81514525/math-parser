using System;
using System.Numerics;

namespace math_parser.math
{
    public class FractionFormatException : FormatException
    {
        public FractionFormatException() : base("Invalid format string at ui.math.Fraction") { }
        public FractionFormatException(string value) : base($"Invalid format string of {value} at ui.math.Fraction") { }
    }

    public static class FracConverter
    {
        public static Fraction ParseInteger(string integer)
        {
            if (!RegexChecker.IsInteger(integer))
                throw new FractionFormatException(integer);
            return new Fraction(BigInteger.Parse(integer), 1);
        }

        public static Fraction ParseDecimal(string decimalValue)
        {
            if (!RegexChecker.IsDecimal(decimalValue))
                throw new FractionFormatException(decimalValue);
            string left = decimalValue.Split('.')[0];
            string right = decimalValue.Split('.')[1];
            int rCount = right.Length;
            return new Fraction(BigInteger.Parse(left + right), BigInteger.Parse("1" + new string('0', rCount)));
        }

        public static Fraction ParseNumber(string number)
        {
            if (!RegexChecker.IsNumber(number))
                throw new FractionFormatException(number);
            if (RegexChecker.IsDecimal(number)) return ParseDecimal(number);
            if (RegexChecker.IsInteger(number)) return ParseInteger(number);
            throw new NotSupportedException($"Regex Error: {number} is being consider as number but have not been consider as either of decimal or integer");
        }

        public static Fraction ParseFraction(string fraction)
        {
            if (!RegexChecker.IsFraction(fraction))
                throw new FractionFormatException(fraction);
            string[] lr = fraction.Split('/');
            string left = lr[0];
            string right = lr[1];
            return ParseNumber(left) / ParseNumber(right);
        }

        public static Fraction Parse(string value)
        {
            if (!RegexChecker.IsFracOrNum(value))
                throw new FractionFormatException(value);
            if (RegexChecker.IsFraction(value)) return ParseFraction(value);
            if (RegexChecker.IsNumber(value)) return ParseNumber(value);
            throw new NotSupportedException($"Regex Error: {value} is being consider as parsable but have not been consider as either of fraction or number");
            
        }
    }
}