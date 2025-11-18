namespace math_parser.tokenizer
{

    public class Maybe<TS> : Group<RepeatListResult<TS>> where TS : ParseResult
    {
        public Maybe(IToken<TS> inner) : base(new Repeat<TS>(inner, 0, 1))
        {
            
        }
    }
}