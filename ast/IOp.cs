namespace math_parser.ast
{
    public interface IAdd<I, O>
    {
        O Add(I right);
    }

    public interface ISelfAdd<O>
    {
        O Add();
    }

    public interface ISub<I, O>
    {
        O Sub(I right);
    }

    public interface ISelfSub<O>
    {
        O Sub();
    }

    public interface IMul<I, O>
    {
        O Mul(I right);
    }

    public interface IDiv<I, O>
    {
        O Div(I right);
    }
}