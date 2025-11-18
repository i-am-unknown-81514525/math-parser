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

    public class ArithmeticSymbolAtom : Group<MathAtomResult>
    {
        public ArithmeticSymbolAtom() : base(
            new OrNoBacktrack<MathAtomResult>(
                new MathLiteral("+"),
                new MathLiteral("-"),
                new MathLiteral("*"),
                new MathLiteral("/")
            )
        )
        { }
    }

    public class ComparsionSymbolAtom : Group<MathAtomResult>
    {
        public ComparsionSymbolAtom() : base(
            new OrNoBacktrack<MathAtomResult>(
                new MathLiteral("<="),
                new MathLiteral("="),
                new MathLiteral(">=")
            )
        )
        { }
    }
    
    public class FullComparsionSymbolAtom : Group<MathAtomResult>
    {
        public FullComparsionSymbolAtom() : base(
            new Or<MathAtomResult>(
                new MathLiteral("="),
                new MathLiteral("<="),
                new MathLiteral(">="),
                new MathLiteral("<"),
                new MathLiteral(">")
            )
        ) {}
    }
}