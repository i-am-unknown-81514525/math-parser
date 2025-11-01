namespace math_parser.ast
{
    public class ASTAdd<L, R, O> : ASTOpNode2<L, R, O> where L : IAdd<R, O>
    {
        public ASTAdd(IASTNode<L> left, IASTNode<R> right) : base(left, right) { }

        public override O Calc() => left.Calc().Add(right.Calc());
    }

    public class ASTAdd<I, O> : ASTOpNode2<I, O> where I : IAdd<I, O>
    {
        public ASTAdd(IASTNode<I> left, IASTNode<I> right) : base(left, right) { }

        public override O Calc() => left.Calc().Add(right.Calc());
    } 

    public class ASTAdd<T> : ASTOpNode2<T> where T : IAdd<T, T>
    {
        public ASTAdd(IASTNode<T> left, IASTNode<T> right) : base(left, right) { }

        public override T Calc() => left.Calc().Add(right.Calc());
    } 
}