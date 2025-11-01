namespace math_parser.ast
{
    public class ASTWrapper1<T> : IASTNode<T>
    {
        protected readonly IASTNode<T> inner;

        public ASTWrapper1(IASTNode<T> inner)
        {
            this.inner = inner;
        }

        public T Calc() => inner.Calc();
    }
}