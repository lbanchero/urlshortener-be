using UrlShortener.Data.Models;

namespace UrlShortener.Data.Interfaces;

public interface IClickRepository
{
    Task CreateAsync(Click click);

    Task DeleteByLinkId(Guid linkId);
}