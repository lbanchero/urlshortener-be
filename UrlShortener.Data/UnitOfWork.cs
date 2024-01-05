using UrlShortener.Data.Interfaces;

namespace UrlShortener.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly UrlShortenerDbContext _context;
    
    public UnitOfWork(UrlShortenerDbContext context)
    {
        _context = context;
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}