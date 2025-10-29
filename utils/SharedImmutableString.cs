namespace math_parser.utils
{

    public sealed class SharedImmutableString
    {
        private string content;

        public SharedImmutableString(string content)
        {
            this.content = content;
        }

        public char this[int idx]
        {
            get => this.content[idx];
        }

        public SharedImmutableString(char c, int length)
        {
            this.content = new string(c, length);
        }

        public int Length
        {
            get => this.content.Length;
        }

        public SharedImmutableString Clone()
        {
            return this;
        }

        public static implicit operator SharedImmutableString(string v)
        {
            return new SharedImmutableString(v);
        }

        public string SubString(int startIndex, int length)
        {
            return this.content.Substring(startIndex, length);
        }

        public string SubString(int startIndex)
        {
            return this.content.Substring(startIndex);
        }
    }
}