using UrlShortener.Data.Models;

namespace UrlShortener.Data.Interfaces;

public interface ILinkRepository
{
    Task CreateAsync(Link? link);
    Task<Link?> GetByShortCodeAsync(string shortCode);
    void DeleteAsync(Link link);
}