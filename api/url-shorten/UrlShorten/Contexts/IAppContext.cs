namespace UrlShorten.Contexts
{
    using Microsoft.EntityFrameworkCore;
    using UrlShorten.Models;

    public interface IAppContext : IDisposable
    {
        DbSet<ShortUrl> Url { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
