namespace UrlShorten.Tests
{
    using NUnit.Framework;
    using UrlShorten.Services;

    [TestFixture]
    public class UrlValidatorTests
    {
        private IUrlValidator _urlValidator;

        [SetUp]
        public void Setup()
        {
            _urlValidator = new UrlValidator();
        }

        [TestCase("https://www.test.com/", ExpectedResult = "https://www.test.com/")]
        [TestCase("www.test.com", ExpectedResult = "http://www.test.com/")]
        [TestCase("test.org", ExpectedResult = "http://test.org/")]
        public string ValidateUrl_Returns_Fully_Formed_Urls(string url)
        {
            return _urlValidator.ValidateUrl(url);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("test")]
        public void ValidateUrl_Returns_Null_On_Invalid_Url_Format(string url)
        {
            var actual = _urlValidator.ValidateUrl(url);

            Assert.AreEqual(actual, null);
        }
    }
}