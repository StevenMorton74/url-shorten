namespace UrlShorten.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using UrlShorten.Contexts;
    using UrlShorten.Models;
    using UrlShorten.Services;

    [ApiController]
    [Route("")]
    public class UrlController : ControllerBase
    {
        private readonly ILogger<UrlController> _logger;
        private readonly IUrlValidator _urlValidator;
        private readonly IEncoder _encoder;
        private readonly IAppContext _context;
        private readonly string _hostname;

        public UrlController(
            ILogger<UrlController> logger,
            IUrlValidator urlValidator,
            IEncoder encoder,
            IAppContext context,
            IConfiguration config)
        {
            _logger = logger;
            _urlValidator = urlValidator;
            _encoder = encoder;
            _context = context;
            _hostname = config["Hostname"];
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ShortenResponse>> CreateShortUrl([FromBody] ShortenRequest request)
        {
            if (string.IsNullOrEmpty(request.Url))
            {
                return BadRequest($"No url provided. Please provide a valid url and try again.");
            }

            var validUrl = _urlValidator.ValidateUrl(request.Url);

            if (validUrl == null)
            {
                return BadRequest($"The url you have provided is invalid. Please provide a valid url and try again.");
            }

            var shortenedUrl = new ShortUrl()
            {
                Url = validUrl,
                Code = Guid.NewGuid().ToString(),
            };

            using var transaction = _context.BeginTransaction();

            try
            {
                _context.Url.Add(shortenedUrl);
                await _context.SaveChangesAsync();

                shortenedUrl.Code = _encoder.Encode(shortenedUrl.Id);
                await _context.SaveChangesAsync();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError("Error occured during transaction. {message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An internal server error occurred. Please try again later.");
            }
            finally
            {
                transaction.Dispose();
            }

            var response = new ShortenResponse()
            {
                ShortUrl = $"{_hostname}/{shortenedUrl.Code}",
            };

            return response;
        }

        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetByCode(string code)
        {
            var shortUrl = await _context.Url.FirstOrDefaultAsync(x => x.Code == code);

            if (shortUrl == null || shortUrl.Url == null)
            {
                _logger.LogDebug("Unable to find url with code {code}", code);
                return NotFound($"Unable to find a url with that code.");
            }

            return Redirect(shortUrl.Url);
        }
    }
}