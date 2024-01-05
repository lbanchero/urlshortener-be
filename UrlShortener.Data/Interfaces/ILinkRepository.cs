using UrlShortener.Data.Models;

namespace UrlShortener.Data.Interfaces;

public interface ILinkRepository
{
    Task CreateAsync(Link? link);
    Task<Link?> GetByShortCodeAsync(string shortCode);
    Task<Link?> GetByShortCodeWithClicksAsync(string shortCode);
    void Update(Link link);
    void DeleteAsync(Link link);
}