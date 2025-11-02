using System.Collections.Generic;

namespace math_parser.tokenizer
{
    public static class Keyword
    {
        public static List<string> keywords = new List<string>();

        public static void PushKeyword(string keyword)
        {
            keywords.Add(keyword);
        }

        public static bool IsStartWithKeyword(string contents)
        {
            foreach (string keyword in keywords)
            {
                if (contents.StartsWith(keyword)) return true;
            }
            return false;
        }

        public static bool IsKeyword(string contents)
        {
            return keywords.Contains(contents);
        }
    }
}