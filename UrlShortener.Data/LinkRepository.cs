using Microsoft.EntityFrameworkCore;
using UrlShortener.Data.Interfaces;
using UrlShortener.Data.Models;

namespace UrlShortener.Data;

public class LinkRepository : ILinkRepository
{
    private readonly UrlShortenerDbContext _context;

    public LinkRepository(UrlShortenerDbContext context)
    {
        _context = context;
    }
    
    public async Task CreateAsync(Link? link)
    {
        await _context.Links.AddAsync(link);
    }

    public async Task<Link?> GetByShortCodeAsync(string shortCode)
    {
        return await GetByShortCode(shortCode).FirstOrDefaultAsync();
    }
    
    public async Task<Link?> GetByShortCodeWithClicksAsync(string shortCode)
    {
        return await GetByShortCode(shortCode)
            .Include(x => x.Clicks)
            .FirstOrDefaultAsync();
    }

    public void Update(Link link)
    {
        _context.Links.Update(link);
    }

    public void DeleteAsync(Link link)
    {
        _context.Links.Remove(link);
    }
    
    private IQueryable<Link> GetByShortCode(string shortCode) => _context.Links.Where(x => x.ShortCode == shortCode);
}