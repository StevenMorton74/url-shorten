namespace UrlShorten.Tests
{
    using UrlShorten.Services;
    using Xunit;

    public class RandomIdFactoryTests
    {
        private IIdFactory idFactory;

        public RandomIdFactoryTests()
        {
            idFactory = new RandomIdFactory();
        }

        [Fact]
        public void CreateId_Generates_A_String_With_A_Length_Of_8()
        {
            // Arrange
            var expectedLength = 8;

            // Act
            var result = idFactory.CreateId();

            // Assert
            Assert.Equal(expectedLength, result.Length);
        }
    }
}