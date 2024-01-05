using UrlShortener.Data.Models;

namespace UrlShortener.Business.Interfaces;

public interface ILinkService
{
    public Task<Link> CreateAsync(string url);
    
    public Task<Link> GetAsync(string shortCode);
    
    public Task<Link> GetStatsAsync(string shortUrl);
    
    public Task DeleteAsync(string shortCode);
}