using System.Security.Claims;
using System.Text;
using Serilog;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();

        // Read request body
        string bodyAsText = "";
        if (context.Request.ContentLength > 0)
        {
            using var reader = new StreamReader(
                context.Request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);

            bodyAsText = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        // Extract user info
        string? userEmail = context.User.FindFirst(ClaimTypes.Email)?.Value;
        string? userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;
        string? userId = context.User.FindFirst("UserId")?.Value;

        // Collect headers
        var headers = string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}: {h.Value}"));

        // Log everything
        Log.Information(
            "HTTP {Method} {Path} | User: {Email} ({Role}, ID: {Id}) | Body: {Body}",
            context.Request.Method,
            context.Request.Path,
            userEmail ?? "Anonymous",
            userRole ?? "N/A",
            userId ?? "N/A",
            //headers,
            bodyAsText
        );

        await _next(context);
    }
}