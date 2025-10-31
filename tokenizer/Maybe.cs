namespace math_parser.tokenizer
{

    public class Maybe<S> : Group<ParseResultList<S>> where S : ParseResult
    {
        public Maybe(IToken<S> inner) : base(new Repeat<S>(inner, 0, 1))
        {
            
        }
    }
}