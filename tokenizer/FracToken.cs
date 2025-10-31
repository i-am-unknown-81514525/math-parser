using math_parser.math;

namespace math_parser.tokenizer
{
    public class FracResult : MathAtomResult
    {
        public readonly NumberResult top;
        public readonly NumberResult bottom;

        public FracResult(NumberResult top, NumberResult bottom)
        {
            this.top = top;
            this.bottom = bottom;
        }

        public Fraction AsFraction() => top.AsFraction() / bottom.AsFraction();
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
            MathAtomResult[] v = inner_token.Parse(stream).parseResult;
            NumberResult top = (NumberResult)v[0];
            NumberResult bottom = (NumberResult)v[2];
            return new FracResult(top, bottom);
        }
    }
}