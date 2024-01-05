using UrlShortener.Business;
using UrlShortener.Data;
using UrlShortener.Http.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDataServices();
builder.Services.AddBusinessServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("OpenCorsPolicy", corsPolicyBuilder =>
    {
        corsPolicyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.MapHealthChecks("/healthz");
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("OpenCorsPolicy");

app.UseAuthorization();
app.MapControllers();
app.Run();