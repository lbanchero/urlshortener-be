namespace UrlShortener.Data.Models;

public class Link
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public string ShortCode { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    public List<Click> Clicks { get; set; }
}