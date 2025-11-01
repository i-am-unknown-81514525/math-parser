using math_parser.tokenizer;

namespace math_parser.atom
{
    public class Value : Atom
    {
        public readonly ExprResult inner;

        public Value(ExprResult inner)
        {
            this.inner = inner;
        }
    }
}