namespace math_parser.ast
{
    public abstract class ASTValue1<T> : IASTNode<T>
    {
        protected readonly T value;

        public ASTValue1(T value)
        {
            this.value = value;
        }

        public abstract T Calc();
    }
}