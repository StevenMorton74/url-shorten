namespace UrlShorten.Tests
{
    using UrlShorten.Services;
    using Xunit;

    public class UrlValidatorTests
    {
        private IUrlValidator _urlValidator;

        public UrlValidatorTests()
        {
            _urlValidator = new UrlValidator();
        }

        [Theory]
        [InlineData("https://www.test.com/", "https://www.test.com/")]
        [InlineData("www.test.com", "http://www.test.com/")]
        [InlineData("test.org", "http://test.org/")]
        public void ValidateUrl_Returns_Fully_Formed_Urls(string url, string expected)
        {
            // Act
            var actual = _urlValidator.ValidateUrl(url);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("test")]
        public void ValidateUrl_Returns_Null_On_Invalid_Url_Format(string url)
        {
            // Act
            var actual = _urlValidator.ValidateUrl(url);

            // Assert
            Assert.Null(actual);
        }
    }
}