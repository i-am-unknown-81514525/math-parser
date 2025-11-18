namespace math_parser.ast
{
    public abstract class AstOpNode2<T> : IastNode<T>
    {
        protected readonly IastNode<T> Left;
        protected readonly IastNode<T> Right;

        public AstOpNode2(IastNode<T> left, IastNode<T> right)
        {
            this.Left = left;
            this.Right = right;
        }

        public abstract T Calc();
    }

    public abstract class AstOpNode2<T, TO> : IastNode<TO>
    {
        public readonly IastNode<T> Left;
        public readonly IastNode<T> Right;

        public AstOpNode2(IastNode<T> left, IastNode<T> right)
        {
            this.Left = left;
            this.Right = right;
        }

        public abstract TO Calc();
    }

    public abstract class AstOpNode2<TL, TR, TO> : IastNode<TO>
    {
        protected readonly IastNode<TL> Left;
        protected readonly IastNode<TR> Right;

        public AstOpNode2(IastNode<TL> left, IastNode<TR> right)
        {
            this.Left = left;
            this.Right = right;
        }

        public abstract TO Calc();
    }
}