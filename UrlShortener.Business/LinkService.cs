using System.Text;
using UrlShortener.Business.Exceptions;
using UrlShortener.Business.Interfaces;
using UrlShortener.Data.Interfaces;
using UrlShortener.Data.Models;

namespace UrlShortener.Business;

public class LinkService : ILinkService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILinkRepository _linkRepository;
    private static readonly Random Random = new();
    private const string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public LinkService(ILinkRepository linkRepository, IUnitOfWork unitOfWork)
    {
        _linkRepository = linkRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Link?> Create(string url)
    {
        var link = new Link
        {
            Id = Guid.NewGuid(),
            Url = url,
            ShortCode = GenerateShortUrl(),
            CreatedAt = DateTimeOffset.UtcNow
        };
        
        await _linkRepository.CreateAsync(link);
        
        await _unitOfWork.SaveChangesAsync();

        return link;
    }

    public async Task<Link?> GetAsync(string shortCode)
    {
        return await _linkRepository.GetByShortCodeAsync(shortCode);
    }

    public async Task<Link> GetStats(string shortUrl)
    {
        var link = await _linkRepository.GetByShortCodeAsync(shortUrl);
        
        if (link == null) throw new LinkNotFoundException();

        //TODO: Refactor this to return a specific model.
        return link;
    }

    public async Task Delete(string shortCode)
    {
        var link = await _linkRepository.GetByShortCodeAsync(shortCode);

        if (link == null) throw new LinkNotFoundException();
        
        _linkRepository.DeleteAsync(link);
        
        await _unitOfWork.SaveChangesAsync();
    }
    
    private static string GenerateShortUrl(int length = 6)
    {
        var stringBuilder = new StringBuilder(length);
    
        for (var i = 0; i < length; i++)
        {
            stringBuilder.Append(Chars[Random.Next(Chars.Length)]);
        }
    
        return stringBuilder.ToString();
    }
}