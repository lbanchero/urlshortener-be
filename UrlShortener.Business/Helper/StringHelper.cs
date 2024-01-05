using System.Text;

namespace UrlShortener.Business.Extensions;

public static class StringHelper
{
    private const string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private static readonly Random Random = new Random();

    public static string GenerateCode(int length = 6)
    {
        var stringBuilder = new StringBuilder(length);
    
        for (var i = 0; i < length; i++)
        {
            stringBuilder.Append(Chars[Random.Next(Chars.Length)]);
        }
    
        return stringBuilder.ToString();
    }
}