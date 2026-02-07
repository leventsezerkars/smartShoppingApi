namespace smartShoppingProject.Api.Middleware;

using System.Net;
using System.Text.Json;
using FluentValidation;
using smartShoppingProject.Application.Common.Responses;
using smartShoppingProject.Domain.Exceptions;

/// <summary>
/// Yakalanmamış exception'ları tek noktadan yakalar; yanıt her zaman Response ile aynı formatta (Success, ErrorMessage, Data).
/// Teknik log ILogger (Serilog) ile atılır.
/// </summary>
public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (statusCode, response) = ex switch
        {
            ValidationException vex => (
                HttpStatusCode.BadRequest,
                Response<object?>.Fail(string.Join(" ", vex.Errors.Select(e => e.ErrorMessage)))),
            DomainException => (HttpStatusCode.BadRequest, Response<object?>.Fail(ex.Message)),
            _ => (
                HttpStatusCode.InternalServerError,
                Response<object?>.Fail(ex.Message))
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(ex, "Yakalanmamış hata: {Message}", ex.Message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(response, JsonOptions);
        await context.Response.WriteAsync(json);
    }
}
