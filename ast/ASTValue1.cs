namespace math_parser.ast
{
    public class ASTValue1<T> : IASTNode<T>
    {
        public readonly T value;

        public ASTValue1(T value)
        {
            this.value = value;
        }

        public T Calc() => value;
    }

    public abstract class ASTValue1<S, T> : IASTNode<T>
    {
        public readonly S value;

        public ASTValue1(S value)
        {
            this.value = value;
        }

        public abstract T Calc();
    }
}