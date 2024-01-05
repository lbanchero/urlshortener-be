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
            await HandleExceptionAsync(httpContext, HttpStatusCode.NotFound);
        }
        catch (ArgumentException)
        {
            await HandleExceptionAsync(httpContext, HttpStatusCode.BadRequest);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            context.Response.StatusCode,
            Message = statusCode switch
            {
                HttpStatusCode.NotFound => "Resource not found",
                HttpStatusCode.BadRequest => "Request is invalid",
                _ => "An error occurred"
            }
        };

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}