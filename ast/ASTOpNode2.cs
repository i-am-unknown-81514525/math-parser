namespace math_parser.ast
{
    public abstract class ASTOpNode2<T> : IASTNode<T>
    {
        protected readonly IASTNode<T> left;
        protected readonly IASTNode<T> right;

        public ASTOpNode2(IASTNode<T> left, IASTNode<T> right)
        {
            this.left = left;
            this.right = right;
        }

        public abstract T Calc();
    }
}