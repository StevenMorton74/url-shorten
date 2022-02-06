namespace UrlShorten.Tests
{
    using NUnit.Framework;
    using UrlShorten.Services;

    [TestFixture]
    public class RandomIdFactoryTests
    {
        private IIdFactory idFactory;

        [SetUp]
        public void Setup()
        {
            idFactory = new RandomIdFactory();
        }

        [Test]
        public void CreateId_Generates_A_String_With_A_Length_Of_8()
        {
            // Act
            var result = idFactory.CreateId();

            // Assert
            Assert.That(result.Length == 8);
        }
    }
}