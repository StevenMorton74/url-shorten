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

        public IDbContextTransactionProxy BeginTransaction()
        {
            return new DbContextTransactionProxy(this);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShortUrl>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.Url).IsRequired();
                entity.Property(e => e.Code).IsRequired();
            });
        }
    }
}
