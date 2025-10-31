using math_parser.math;

namespace math_parser.tokenizer
{
    public struct FracResult : MathAtomResult
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
}