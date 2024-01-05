namespace UrlShortener.Data.Models;

public class Click
{
    public Guid Id { get; set; }
    public Guid LinkId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}