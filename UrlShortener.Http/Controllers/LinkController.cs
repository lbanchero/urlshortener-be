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
    
    [HttpGet("{shortUrl}")]
    public async Task<IActionResult> Get(string shortUrl)
    {
        var link = await _linkService.GetAsync(shortUrl);
        
        if (link == null) return NotFound();
        
        return Ok(link.Url);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]ShortenRequest request)
    {
        var link = await _linkService.Create(request.Url);
        
        return Ok(link);
    }
    
    [HttpGet("{shortUrl}/stats")]
    public async Task<IActionResult> GetStats(string shortUrl)
    {
        var link = await _linkService.GetStats(shortUrl);
        
        if (link == null) return NotFound();
        
        return Ok(link);
    }
    
    [HttpDelete("{shortUrl}")]
    public async Task<IActionResult> Delete(string shortUrl)
    {
        await _linkService.Delete(shortUrl);
        
        return Ok();
    }
}