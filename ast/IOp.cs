namespace math_parser.ast
{
    public interface IAdd<TI, TO>
    {
        TO Add(TI right);
    }

    public interface ISelfAdd<TO>
    {
        TO Add();
    }

    public interface ISub<TI, TO>
    {
        TO Sub(TI right);
    }

    public interface ISelfSub<TO>
    {
        TO Sub();
    }

    public interface IMul<TI, TO>
    {
        TO Mul(TI right);
    }

    public interface IDiv<TI, TO>
    {
        TO Div(TI right);
    }
}