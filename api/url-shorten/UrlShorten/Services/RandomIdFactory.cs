namespace UrlShorten.Services
{
    public class RandomIdFactory : IIdFactory
    {
        private const string _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int _length = 8;
        private readonly Random _random;

        public RandomIdFactory()
        {
            _random = new Random();
        }

        public string CreateId()
        {
            var randomString = new string(
                Enumerable.Repeat(_chars, _length)
                          .Select(s => s[_random.Next(s.Length)]).ToArray());
            return randomString;
        }
    }
}
