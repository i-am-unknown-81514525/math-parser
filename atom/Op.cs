namespace math_parser.atom
{
    public interface SymbolAtom : Atom { }
    public class ArithematicSymbolAtom : SymbolAtom
    {
        public readonly string literal;
        private ArithematicSymbolAtom(string v)
        {
            literal = v;
        }

        public static readonly ArithematicSymbolAtom Add = new ArithematicSymbolAtom("+");
        public static readonly ArithematicSymbolAtom Sub = new ArithematicSymbolAtom("-");
        public static readonly ArithematicSymbolAtom Mul = new ArithematicSymbolAtom("*");
        public static readonly ArithematicSymbolAtom Div = new ArithematicSymbolAtom("/");
    }

    public class ComparsionSymbolAtom : SymbolAtom
    {
        public readonly string literal;
        private ComparsionSymbolAtom(string v)
        {
            literal = v;
        }

        public static readonly ComparsionSymbolAtom Eq = new ComparsionSymbolAtom("=");
        public static readonly ComparsionSymbolAtom Le = new ComparsionSymbolAtom("<=");
        public static readonly ComparsionSymbolAtom Ge = new ComparsionSymbolAtom(">=");
    }
}