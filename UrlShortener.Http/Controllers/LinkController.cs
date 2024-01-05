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
    /// <param name="shortCode"></param>
    /// <returns></returns>
    [HttpGet("{shortCode}")]
    public async Task<IActionResult> Get(string shortCode)
    {
        var link = await _linkService.GetAsync(shortCode);
        
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
    /// <param name="shortCode"></param>
    /// <returns></returns>
    [HttpGet("{shortCode}/stats")]
    public async Task<IActionResult> GetStats(string shortCode)
    {
        var link = await _linkService.GetStatsAsync(shortCode);

        return Ok(link.Clicks);
    }
    
    /// <summary>
    /// Deletes a URL shortening object with the associated statistics
    /// </summary>
    /// <param name="shortCode"></param>
    /// <returns></returns>
    [HttpDelete("{shortCode}")]
    public async Task<IActionResult> Delete(string shortCode)
    {
        await _linkService.DeleteAsync(shortCode);
        
        return Ok();
    }
}