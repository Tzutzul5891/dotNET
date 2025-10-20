using System.Net;
using System.Text.Json;
using BookApi.Common.Exceptions;

namespace BookApi.Common.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var (statusCode, message, data) = exception switch
        {
            BookNotFoundException bookNotFound => 
                ((int)HttpStatusCode.NotFound, bookNotFound.Message, (object)new { BookId = bookNotFound.BookId }),
            DuplicateIsbnException duplicateIsbn => 
                ((int)HttpStatusCode.Conflict, duplicateIsbn.Message, (object)new { Isbn = duplicateIsbn.Isbn }),
            ValidationException validationEx => 
                ((int)HttpStatusCode.BadRequest, validationEx.Message, (object)new { Errors = validationEx.Errors }),
            InvalidBookOperationException invalidOp => 
                ((int)HttpStatusCode.BadRequest, invalidOp.Message, (object)new { }),
            _ => 
                ((int)HttpStatusCode.InternalServerError, "An internal server error occurred. Please try again later.", (object)new { })
        };

        context.Response.StatusCode = statusCode;
        
        var response = new
        {
            StatusCode = statusCode,
            Message = message,
            Data = data
        };

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}
