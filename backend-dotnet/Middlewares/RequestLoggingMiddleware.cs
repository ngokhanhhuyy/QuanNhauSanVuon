using System.Globalization;
using QuanNhauSanVuon.Extensions;

namespace QuanNhauSanVuon.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
        string method = context.Request.Method;
        string path = context.Request.Path;
        QueryString queryString = context.Request.QueryString;
        int statusCode = context.Response.StatusCode;
        string statusColor = statusCode switch
        {
            >= 200 and < 300 => "\u001b[42m\u001b[37m", // Green
            >= 300 and < 400 => "\u001b[43m\u001b[30m", // Yellow
            >= 400 and < 500 => "\u001b[41m\u001b[37m", // Red
            >= 500 => "\u001b[46m\u001b[30m",           // Cyan
            _ => ""
        };
        string currentDateTimeAsString = DateTime.UtcNow
            .ToApplicationTime()
            .ToString(CultureInfo.InvariantCulture);

        string logEntry =
            $"{statusColor}{statusCode}\u001b[0m     " +
            $"\u001b[47m\u001b[30m{currentDateTimeAsString}\u001b[0m " +
            $"{method} {path}{queryString}";

        Console.WriteLine(logEntry);
    }
}