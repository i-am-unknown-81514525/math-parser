using math_parser.tokenizer;

namespace math_parser.atom
{
    public class Value : Atom
    {
        public readonly ExprResult Inner;

        public Value(ExprResult inner)
        {
            Inner = inner;
        }
    }
}