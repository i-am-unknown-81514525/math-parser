namespace math_parser.utils
{

    public sealed class SharedImmutableString
    {
        private string _content;

        public SharedImmutableString(string content)
        {
            _content = content;
        }

        public char this[int idx]
        {
            get => _content[idx];
        }

        public SharedImmutableString(char c, int length)
        {
            _content = new string(c, length);
        }

        public int Length
        {
            get => _content.Length;
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
            if (length <= 0)
            {
                return "";
            }
            if (startIndex + 1 >= Length)
            {
                return "";
            }
            if (startIndex + length >= Length)
            {
                length = Length - startIndex;
            }
            return _content.Substring(startIndex, length);
        }

        public string SubString(int startIndex)
        {
            if (startIndex + 1 >= _content.Length)
            {
                return "";
            }
            return _content.Substring(startIndex);
        }

        public string AsRaw() {
            return _content;
        }
    }
}