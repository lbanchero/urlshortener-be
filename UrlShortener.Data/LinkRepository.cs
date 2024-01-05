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
        return await _context.Links.FirstOrDefaultAsync(x => x.ShortCode == shortCode);
    }

    public void DeleteAsync(Link link)
    {
        _context.Remove(link);
    }
}