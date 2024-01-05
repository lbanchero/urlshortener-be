using Microsoft.EntityFrameworkCore;
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
    
    public async Task DeleteByLinkId(Guid linkId)
    {
        var clicks = await _context.Clicks.Where(x => x.LinkId == linkId).ToListAsync();

        _context.Clicks.RemoveRange(clicks);
    }
}