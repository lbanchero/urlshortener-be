namespace UrlShortener.Data.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}