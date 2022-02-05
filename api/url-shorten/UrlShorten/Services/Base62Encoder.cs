namespace UrlShorten.Services
{
    using Base62;

    public class Base62Encoder : IEncoder
    {
        private readonly Base62Converter _base62Converter;

        public Base62Encoder()
        {
            _base62Converter = new Base62Converter();
        }

        public string Encode(int number)
        {
            if (number < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(number));
            }

            return _base62Converter.Encode(number.ToString("D8"));
        }
    }
}
