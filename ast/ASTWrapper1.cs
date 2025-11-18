namespace math_parser.ast
{
    public class AstWrapper1<T> : IastNode<T>
    {
        public readonly IastNode<T> Inner;

        public AstWrapper1(IastNode<T> inner)
        {
            this.Inner = inner;
        }

        public T Calc() => Inner.Calc();
    }
}