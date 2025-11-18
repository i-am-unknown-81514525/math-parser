namespace math_parser.utils
{

    public sealed class SharedImmutableString
    {
        private string _content;

        public SharedImmutableString(string content)
        {
            this._content = content;
        }

        public char this[int idx]
        {
            get => this._content[idx];
        }

        public SharedImmutableString(char c, int length)
        {
            this._content = new string(c, length);
        }

        public int length
        {
            get => this._content.Length;
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
            if (startIndex + 1 >= _content.Length)
            {
                return "";
            }
            if (startIndex + length >= _content.Length)
            {
                length = _content.Length - startIndex;
            }
            return this._content.Substring(startIndex, length);
        }

        public string SubString(int startIndex)
        {
            if (startIndex + 1 >= _content.Length)
            {
                return "";
            }
            return this._content.Substring(startIndex);
        }

        public string AsRaw() {
            return this._content;
        }
    }
}