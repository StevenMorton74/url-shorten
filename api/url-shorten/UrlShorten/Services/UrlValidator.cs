namespace UrlShorten.Services
{
    using System.Text.RegularExpressions;

    public class UrlValidator : IUrlValidator
    {
        private const string RegexPattern = @"[(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)";

        public string ValidateUrl(string url)
        {
            if (url == null)
            {
                return null;
            }

            Match validUri = Regex.Match(url, RegexPattern, RegexOptions.IgnoreCase);
            if (!validUri.Success)
            {
                return null;
            }

            return new UriBuilder(url).Uri.ToString();
        }
    }
}
