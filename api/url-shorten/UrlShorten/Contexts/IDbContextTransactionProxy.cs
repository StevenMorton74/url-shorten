namespace UrlShorten.Contexts
{
    public interface IDbContextTransactionProxy : IDisposable
    {
        void Commit();

        void Rollback();
    }
}
