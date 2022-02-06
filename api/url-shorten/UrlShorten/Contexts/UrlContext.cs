namespace UrlShorten.Contexts
{
    using Microsoft.EntityFrameworkCore;
    using UrlShorten.Models;

    public class UrlContext : DbContext, IAppContext
    {
        public UrlContext(DbContextOptions<UrlContext> options) : base(options)
        {
        }

        public virtual DbSet<ShortUrl> Url { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShortUrl>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Url).IsRequired();
            });
        }
    }
}
