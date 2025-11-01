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

    public class ASTSub<L, R, O> : ASTOpNode2<L, R, O> where L : ISub<R, O>
    {
        public ASTSub(IASTNode<L> left, IASTNode<R> right) : base(left, right) { }

        public override O Calc() => left.Calc().Sub(right.Calc());
    }

    public class ASTSub<I, O> : ASTOpNode2<I, O> where I : ISub<I, O>
    {
        public ASTSub(IASTNode<I> left, IASTNode<I> right) : base(left, right) { }

        public override O Calc() => left.Calc().Sub(right.Calc());
    }

    public class ASTSub<T> : ASTOpNode2<T> where T : ISub<T, T>
    {
        public ASTSub(IASTNode<T> left, IASTNode<T> right) : base(left, right) { }

        public override T Calc() => left.Calc().Sub(right.Calc());
    } 

    public class ASTMul<L, R, O> : ASTOpNode2<L, R, O> where L : IMul<R, O>
    {
        public ASTMul(IASTNode<L> left, IASTNode<R> right) : base(left, right) { }

        public override O Calc() => left.Calc().Mul(right.Calc());
    }

    public class ASTMul<I, O> : ASTOpNode2<I, O> where I : IMul<I, O>
    {
        public ASTMul(IASTNode<I> left, IASTNode<I> right) : base(left, right) { }

        public override O Calc() => left.Calc().Mul(right.Calc());
    } 

    public class ASTMul<T> : ASTOpNode2<T> where T : IMul<T, T>
    {
        public ASTMul(IASTNode<T> left, IASTNode<T> right) : base(left, right) { }

        public override T Calc() => left.Calc().Mul(right.Calc());
    } 
}