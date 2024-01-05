using System.Text;
using UrlShortener.Business.Exceptions;
using UrlShortener.Business.Extensions;
using UrlShortener.Business.Interfaces;
using UrlShortener.Data.Interfaces;
using UrlShortener.Data.Models;

namespace UrlShortener.Business;

public class LinkService : ILinkService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILinkRepository _linkRepository;
    private readonly IClickRepository _clickRepository;

    public LinkService(ILinkRepository linkRepository,
        IClickRepository clickRepository,
        IUnitOfWork unitOfWork)
    {
        _linkRepository = linkRepository;
        _clickRepository = clickRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Link> GetAsync(string shortCode)
    {
        if (string.IsNullOrWhiteSpace(shortCode))
            throw new ArgumentException(null, nameof(shortCode));
        
        var link = await _linkRepository.GetByShortCodeAsync(shortCode);
        
        if (link == null)
            throw new LinkNotFoundException();
        
        await CreateClickAsync(link);

        return link;
    }

    public async Task<Link> CreateAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException(null, nameof(url));
        
        var link = new Link
        {
            Id = Guid.NewGuid(),
            Url = url,
            ShortCode = StringHelper.GenerateCode(),
            CreatedAt = DateTimeOffset.UtcNow
        };
        
        await _linkRepository.CreateAsync(link);
        
        await _unitOfWork.SaveChangesAsync();

        return link;
    }

    public async Task<Link> GetStatsAsync(string shortCode)
    {
        if (string.IsNullOrWhiteSpace(shortCode))
            throw new ArgumentException(null, nameof(shortCode));
        
        var linkWithClicks = await _linkRepository.GetByShortCodeWithClicksAsync(shortCode);
        
        if (linkWithClicks == null) throw new LinkNotFoundException();

        return linkWithClicks;
    }

    public async Task DeleteAsync(string shortCode)
    {
        var link = await _linkRepository.GetByShortCodeAsync(shortCode);

        if (link == null) throw new LinkNotFoundException();

        await _clickRepository.DeleteByLinkId(link.Id);
        
        _linkRepository.DeleteAsync(link);
        
        await _unitOfWork.SaveChangesAsync();
    }
    
    private async Task CreateClickAsync(Link link)
    {
        var click = new Click
        {
            CreatedAt = DateTimeOffset.Now,
            Id = Guid.NewGuid(),
            LinkId = link.Id
        };

        link.Clicks.Add(click);
            
        await _clickRepository.CreateAsync(click);
        
        await _unitOfWork.SaveChangesAsync();
    }
}