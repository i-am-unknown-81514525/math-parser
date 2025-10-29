namespace math_parser.utils
{

    public class SharedImmutableString
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
    }
}