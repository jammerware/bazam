using System.Text;

namespace Bazam.Slugging
{
    public abstract class Slugger
    {
        private string _Key = null;
        protected abstract string SlugBase();

        public static string Slugify(string input)
        {
            StringBuilder builder = new StringBuilder();
            bool prevCharWasDash = false;

            foreach (char c in input.ToLower().ToCharArray())
            {
                char? toAppend;
                if(char.IsLetterOrDigit(c))
                    toAppend = c;
                else if(c == '\'')
                    toAppend = null;
                else
                    toAppend = '-';

                if ((toAppend != '-' || (toAppend == '-' && !prevCharWasDash))) {
                    builder.Append(toAppend.ToString());
                }
                prevCharWasDash = (toAppend == '-');
            }
            return builder.ToString().TrimEnd('-');
        }

        public string GetSlug()
        {
            if (_Key == null)
                _Key = Slugify(SlugBase());
            return _Key;
        }
    }
}