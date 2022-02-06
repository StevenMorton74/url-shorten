namespace UrlShorten.Tests
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using MockQueryable.Moq;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using UrlShorten.Contexts;
    using UrlShorten.Controllers;
    using UrlShorten.Models;
    using UrlShorten.Services;

    [TestFixture]
    public class UrlControllerTests
    {
        private const string _hostname = "https://localhost:7139";
        private UrlController _urlController;

        [SetUp]
        public void Setup()
        {
            // Arrange
            var data = new List<ShortUrl>
            {
                new ShortUrl { Id = 0, Url = "https://test.com", Code = "48VIcsBN5UX" },
                new ShortUrl { Id = 1, Url = "https://test2.com", Code = "48VIcsBN5UY" },
                new ShortUrl { Id = 2, Url = "https://test3.com", Code = "48VIcsBN5UZ" },
            };

            var mockSet = data.AsQueryable().BuildMockDbSet();
            var mockedDbTransaction = new Mock<IDbContextTransactionProxy>();

            var mockContext = new Mock<IAppContext>();
            mockContext.Setup(c => c.Url).Returns(mockSet.Object);
            mockContext.Setup(c => c.BeginTransaction()).Returns(mockedDbTransaction.Object);

            var mockUrlValidator = new Mock<IUrlValidator>();
            mockUrlValidator.Setup(x => x.ValidateUrl(It.IsAny<string>())).Returns("https//www.test.com");

            var mockEncoder = new Mock<IEncoder>();
            mockEncoder.Setup(x => x.Encode(It.IsAny<int>())).Returns("TestCode");

            var mockLogger = Mock.Of<ILogger<UrlController>>();

            var inMemoryConfig = new Dictionary<string, string> {
                {"Hostname", _hostname},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfig)
                .Build();

            _urlController = new UrlController(mockLogger, mockUrlValidator.Object, mockEncoder.Object, mockContext.Object, configuration);
        }

        [Test]
        public async Task CreateShortUrl_Creates_ShortUrl_Response()
        {
            // Arrange
            var request = new ShortenRequest
            {
                Url = "https//www.test.com"
            };

            // Act
            var result = await _urlController.CreateShortUrl(request);

            // Assert
            Assert.IsNotNull(result?.Value?.ShortUrl);
            Assert.That(result.Value.ShortUrl.Equals($"{_hostname}/TestCode"));
        }

        [TestCase("48VIcsBN5UX")]
        [TestCase("48VIcsBN5UY")]
        [TestCase("48VIcsBN5UZ")]
        public async Task GetByCode_Returns_Redirect_If_Found(string code)
        {
            // Act
            var result = await _urlController.GetByCode(code);

            // Assert
            Assert.IsInstanceOf<RedirectResult>(result);
        }

        [TestCase("test")]
        [TestCase("")]
        [TestCase(null)]
        public async Task GetByCode_Returns_Not_Found_If_Not_Found(string code)
        {
            // Act
            var result = await _urlController.GetByCode(code);

            Console.WriteLine(result?.ToString());

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }
    }
}
