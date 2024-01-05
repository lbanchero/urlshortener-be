using UrlShortener.Data.Interfaces;
using UrlShortener.Data.Models;

namespace UrlShortener.Data;

public class ClickRepository : IClickRepository
{
    private readonly UrlShortenerDbContext _context;

    public ClickRepository(UrlShortenerDbContext context)
    {
        _context = context;
    }
    
    public async Task CreateAsync(Click click)
    {
        await _context.Clicks.AddAsync(click);
    }
}