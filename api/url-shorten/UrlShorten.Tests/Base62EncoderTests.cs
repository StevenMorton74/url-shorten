namespace UrlShorten.Tests
{
    using NUnit.Framework;
    using System;
    using UrlShorten.Services;

    [TestFixture]
    public class Base62EncoderTests
    {
        private IEncoder _encoder;

        [SetUp]
        public void Setup()
        {
            _encoder = new Base62Encoder();
        }

        [TestCase(1, ExpectedResult = "48VIcsBN5UX")]
        [TestCase(1000, ExpectedResult = "48VIcsCVU0m")]
        [TestCase(500000000, ExpectedResult = "Ir0eKPoSA5mC")]
        public string Encode_Returns_Base62_Of_Input_Number(int number)
        {
            return _encoder.Encode(number);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void Encode_Throws_Exception_On_Input_Less_Than_One(int number)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _encoder.Encode(number));
        }
    }
}