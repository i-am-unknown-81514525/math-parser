namespace math_parser.tokenizer
{
    public interface IToken
    {
        CharacterStream Parse(CharacterStream stream);
        bool CanParse(CharacterStream stream);

        CharacterStream PartialParse(CharacterStream stream);
        bool CanPartialParse(CharacterStream stream);
    }
}