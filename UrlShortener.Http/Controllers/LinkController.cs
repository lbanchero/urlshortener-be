using Microsoft.AspNetCore.Mvc;
using UrlShortener.Business.Interfaces;
using UrlShortener.Data.Models;
using UrlShortener.Http.Models;

namespace UrlShortener.Http.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LinkController : ControllerBase
{
    private readonly ILinkService _linkService;

    public LinkController(ILinkService linkService)
    {
        _linkService = linkService ?? throw new ArgumentNullException(nameof(linkService));
    }
    
    /// <summary>
    /// Gets the original URL object from the short URL
    /// Creates a new Click object to track activity
    /// </summary>
    /// <param name="shortUrl"></param>
    /// <returns></returns>
    [HttpGet("{shortUrl}")]
    public async Task<IActionResult> Get(string shortUrl)
    {
        var link = await _linkService.GetAsync(shortUrl);
        
        return Ok(link.Url);
    }

    /// <summary>
    /// Creates  a new URL shortening object
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]ShortenRequest request)
    {
        var link = await _linkService.CreateAsync(request.Url);
        
        return Ok(link);
    }
    
    /// <summary>
    /// Gets the statistics for a URL shortening object
    /// </summary>
    /// <param name="shortUrl"></param>
    /// <returns></returns>
    [HttpGet("{shortUrl}/stats")]
    public async Task<IActionResult> GetStats(string shortUrl)
    {
        var link = await _linkService.GetStatsAsync(shortUrl);

        return Ok(link.Clicks);
    }
    
    /// <summary>
    /// Deletes a URL shortening object with the associated statistics
    /// </summary>
    /// <param name="shortUrl"></param>
    /// <returns></returns>
    [HttpDelete("{shortUrl}")]
    public async Task<IActionResult> Delete(string shortUrl)
    {
        await _linkService.DeleteAsync(shortUrl);
        
        return Ok();
    }
}