using System.Text;

namespace math_parser.tokenizer
{
    public interface ParseResult
    {
        string ToString(int indent);
    }

    public static class ParseResultExtensions
    {
        public static string Indent(int count)
        {
            return new string(' ', count * 2);
        }

        public static string Print(this ParseResult result) {
            return result.ToString(0);
        }
    }
}