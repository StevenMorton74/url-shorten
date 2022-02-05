namespace UrlShorten.Contexts
{
    using Microsoft.EntityFrameworkCore;
    using UrlShorten.Models;

    public interface IAppContext : IDisposable
    {
        DbSet<ShortUrl> Url { get; set; }

        int SaveChanges();

        IDbContextTransactionProxy BeginTransaction();
    }
}
