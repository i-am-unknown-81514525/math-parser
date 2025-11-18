using System.Text;
using math_parser.math;

namespace math_parser.tokenizer
{
    public class FracResult : MathAtomResult
    {
        public readonly NumberResult Top;
        public readonly NumberResult Bottom;

        public FracResult(NumberResult top, NumberResult bottom)
        {
            Top = top;
            Bottom = bottom;
        }

        public Fraction AsFraction() => Top.AsFraction() / Bottom.AsFraction();

        public override string ToString() => ToString(0);
        public string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{ParseResultExtensions.Indent(indent)}FracResult:");
            sb.AppendLine($"{ParseResultExtensions.Indent(indent + 1)}Top:");
            sb.Append(Top.ToString(indent + 2));
            sb.AppendLine($"{ParseResultExtensions.Indent(indent + 1)}Bottom:");
            sb.Append(Bottom.ToString(indent + 2));
            return sb.ToString();
        }
    }

    public class FracToken : Group<TokenSequenceResult<MathAtomResult>, FracResult>
    {
        public FracToken() : base(
            new TokenSequence<MathAtomResult>(
                new Number(),
                new MathLiteral("/"),
                new Number()
            )
        ) {}

        public override FracResult Parse(CharacterStream stream)
        {
            MathAtomResult[] v = InnerToken.Parse(stream).ParseResult;
            NumberResult top = (NumberResult)v[0];
            NumberResult bottom = (NumberResult)v[2];
            return new FracResult(top, bottom);
        }
    }
}