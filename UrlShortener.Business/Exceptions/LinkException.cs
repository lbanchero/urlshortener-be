namespace UrlShortener.Business.Exceptions;

public class LinkNotFoundException : Exception
{
    public LinkNotFoundException() : base("Link not found") { }
}