using UrlShortener.Data.Models;

namespace UrlShortener.Business.Interfaces;

public interface ILinkService
{
    public Task<Link?> Create(string url);
    
    public Task<Link?> GetAsync(string shortCode);
    
    public Task<Link> GetStats(string shortUrl);
    
    public Task Delete(string shortCode);
}