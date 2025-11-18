using System.Collections.Generic;

namespace math_parser.tokenizer
{
    public static class Keyword
    {
        public static List<string> Keywords = new List<string>();

        public static void PushKeyword(string keyword)
        {
            if (Keywords.Contains(keyword)) return;
            Keywords.Add(keyword);
        }

        public static bool IsStartWithKeyword(string contents)
        {
            foreach (string keyword in Keywords)
            {
                if (contents.StartsWith(keyword)) return true;
            }
            return false;
        }

        public static bool IsKeyword(string contents)
        {
            return Keywords.Contains(contents);
        }
    }
}