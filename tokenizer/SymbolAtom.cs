namespace math_parser.tokenizer
{
    public class SymbolAtom : Group<MathAtomResult>
    {
        public SymbolAtom() : base(
            new OrNoBacktrack<MathAtomResult>(
                new MathLiteral("+"),
                new MathLiteral("-"),
                new MathLiteral("*"),
                new MathLiteral("/"),
                new MathLiteral("<="),
                new MathLiteral("="),
                new MathLiteral(">=")
            )
        ) {}
    }
}