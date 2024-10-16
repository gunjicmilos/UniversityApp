using System.Net;
using Newtonsoft.Json;

namespace UniversityApp.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var result = JsonConvert.SerializeObject(new
        {
            StatusCode = context.Response.StatusCode,
            Message = "Internal Server Error. Please try again later.",
            Detailed = exception.Message
        });

        return context.Response.WriteAsync(result);
    }
}