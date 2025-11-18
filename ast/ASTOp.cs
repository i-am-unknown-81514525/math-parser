namespace math_parser.ast
{
    public class AstAdd<TL, TR, TO> : AstOpNode2<TL, TR, TO> where TL : IAdd<TR, TO>
    {
        public AstAdd(IastNode<TL> left, IastNode<TR> right) : base(left, right) { }

        public override TO Calc() => Left.Calc().Add(Right.Calc());
    }

    public class AstAdd<TI, TO> : AstOpNode2<TI, TO> where TI : IAdd<TI, TO>
    {
        public AstAdd(IastNode<TI> left, IastNode<TI> right) : base(left, right) { }

        public override TO Calc() => Left.Calc().Add(Right.Calc());
    } 

    public class AstAdd<T> : AstOpNode2<T> where T : IAdd<T, T>
    {
        public AstAdd(IastNode<T> left, IastNode<T> right) : base(left, right) { }

        public override T Calc() => Left.Calc().Add(Right.Calc());
    } 

    public class AstSub<TL, TR, TO> : AstOpNode2<TL, TR, TO> where TL : ISub<TR, TO>
    {
        public AstSub(IastNode<TL> left, IastNode<TR> right) : base(left, right) { }

        public override TO Calc() => Left.Calc().Sub(Right.Calc());
    }

    public class AstSub<TI, TO> : AstOpNode2<TI, TO> where TI : ISub<TI, TO>
    {
        public AstSub(IastNode<TI> left, IastNode<TI> right) : base(left, right) { }

        public override TO Calc() => Left.Calc().Sub(Right.Calc());
    }

    public class AstSub<T> : AstOpNode2<T> where T : ISub<T, T>
    {
        public AstSub(IastNode<T> left, IastNode<T> right) : base(left, right) { }

        public override T Calc() => Left.Calc().Sub(Right.Calc());
    } 

    public class AstMul<TL, TR, TO> : AstOpNode2<TL, TR, TO> where TL : IMul<TR, TO>
    {
        public AstMul(IastNode<TL> left, IastNode<TR> right) : base(left, right) { }

        public override TO Calc() => Left.Calc().Mul(Right.Calc());
    }

    public class AstMul<TI, TO> : AstOpNode2<TI, TO> where TI : IMul<TI, TO>
    {
        public AstMul(IastNode<TI> left, IastNode<TI> right) : base(left, right) { }

        public override TO Calc() => Left.Calc().Mul(Right.Calc());
    }

    public class AstMul<T> : AstOpNode2<T> where T : IMul<T, T>
    {
        public AstMul(IastNode<T> left, IastNode<T> right) : base(left, right) { }

        public override T Calc() => Left.Calc().Mul(Right.Calc());
    } 

    public class AstDiv<TL, TR, TO> : AstOpNode2<TL, TR, TO> where TL : IDiv<TR, TO>
    {
        public AstDiv(IastNode<TL> left, IastNode<TR> right) : base(left, right) { }

        public override TO Calc() => Left.Calc().Div(Right.Calc());
    }

    public class AstDiv<TI, TO> : AstOpNode2<TI, TO> where TI : IDiv<TI, TO>
    {
        public AstDiv(IastNode<TI> left, IastNode<TI> right) : base(left, right) { }

        public override TO Calc() => Left.Calc().Div(Right.Calc());
    } 

    public class AstDiv<T> : AstOpNode2<T> where T : IDiv<T, T>
    {
        public AstDiv(IastNode<T> left, IastNode<T> right) : base(left, right) { }

        public override T Calc() => Left.Calc().Div(Right.Calc());
    }
}