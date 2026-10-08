public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        DateTime startTime = DateTime.UtcNow;

        Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");

        await _next(context);

        var elapsedTime = DateTime.UtcNow - startTime;

        Console.WriteLine($"Response: {context.Response.StatusCode} - {elapsedTime.TotalMilliseconds}ms");
    }
}