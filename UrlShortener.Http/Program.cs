using UrlShortener.Business;
using UrlShortener.Data;
using UrlShortener.Http.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
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

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("OpenCorsPolicy");

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();
app.Run();