namespace UrlShorten.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using UrlShorten.Contexts;
    using UrlShorten.Models;
    using UrlShorten.Services;

    [ApiController]
    [Route("")]
    public class UrlController : ControllerBase
    {
        private readonly ILogger<UrlController> _logger;
        private readonly IUrlValidator _urlValidator;
        private readonly IIdFactory _idFactory;
        private readonly IAppContext _context;
        private readonly string _hostname;

        public UrlController(
            ILogger<UrlController> logger,
            IUrlValidator urlValidator,
            IIdFactory idFactory,
            IAppContext context,
            IConfiguration config)
        {
            _logger = logger;
            _urlValidator = urlValidator;
            _idFactory = idFactory;
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

            var id = _idFactory.CreateId();

            while(await _context.Url.FindAsync(id) != null)
            {
                id = _idFactory.CreateId();
            }

            var shortUrl = new ShortUrl
            {
                Id = id,
                Url = validUrl,
            };

            try
            {
                _context.Url.Add(shortUrl);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured during transaction. {message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An internal server error occurred. Please try again later.");
            }

            var response = new ShortenResponse()
            {
                ShortUrl = $"{_hostname}/{shortUrl.Id}",
            };

            return response;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetById(string id)
        {
            var shortUrl = await _context.Url.FindAsync(id);

            if (shortUrl == null || shortUrl.Url == null)
            {
                _logger.LogDebug("Unable to find url with code {id}", id);
                return NotFound($"Unable to find a url with that code.");
            }

            return Redirect(shortUrl.Url);
        }
    }
}