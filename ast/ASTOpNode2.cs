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

    public abstract class ASTOpNode2<T, O> : IASTNode<O>
    {
        protected readonly IASTNode<T> left;
        protected readonly IASTNode<T> right;

        public ASTOpNode2(IASTNode<T> left, IASTNode<T> right)
        {
            this.left = left;
            this.right = right;
        }

        public abstract O Calc();
    }

    public abstract class ASTOpNode2<L, R, O> : IASTNode<O>
    {
        protected readonly IASTNode<L> left;
        protected readonly IASTNode<R> right;

        public ASTOpNode2(IASTNode<L> left, IASTNode<R> right)
        {
            this.left = left;
            this.right = right;
        }

        public abstract O Calc();
    }
}