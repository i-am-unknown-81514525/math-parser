namespace math_parser.ast
{
    public class AstValue1<T> : IastNode<T>
    {
        public readonly T Value;

        public AstValue1(T value)
        {
            this.Value = value;
        }

        public T Calc() => Value;
    }

    public abstract class AstValue1<TS, T> : IastNode<T>
    {
        public readonly TS Value;

        public AstValue1(TS value)
        {
            this.Value = value;
        }

        public abstract T Calc();
    }
}