using System.Net;
using Newtonsoft.Json;
using UrlShortener.Business.Exceptions;

namespace UrlShortener.Http.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (LinkNotFoundException)
        {
            await HandleNotFoundExceptionAsync(httpContext);
        }
    }

    private static Task HandleNotFoundExceptionAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = "Resource not found"
        };

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}