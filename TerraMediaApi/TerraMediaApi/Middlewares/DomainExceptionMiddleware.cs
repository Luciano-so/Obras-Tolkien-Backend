using System.Net;
using System.Text.Json;
using TerraMedia.Application.Dtos;
using TerraMedia.Domain.Exceptions;

namespace TerraMedia.Api.Middlewares;

public class DomainExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public DomainExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (DomainException ex)
        {
            await HandleDomainExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleDomainExceptionAsync(HttpContext context, DomainException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var response = ReponseDto.Create(HttpStatusCode.BadRequest, exception.Message);

        var json = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(json);
    }
}
