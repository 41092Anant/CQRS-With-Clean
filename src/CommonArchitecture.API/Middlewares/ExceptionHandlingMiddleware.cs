using System.Net;
using System.Text.Json;

namespace CommonArchitecture.API.Middlewares;

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
 _logger.LogError(ex, "Unhandled exception occurred while processing request {Method} {Path}", context.Request.Method, context.Request.Path);
 await HandleExceptionAsync(context, ex);
 }
 }

 private static Task HandleExceptionAsync(HttpContext context, Exception exception)
 {
 var code = HttpStatusCode.InternalServerError;

 // Here you can handle different exception types and set specific status codes
 // e.g., if (exception is UnauthorizedAccessException) code = HttpStatusCode.Unauthorized;

 var problem = new
 {
 success = false,
 message = "An unexpected error occurred. Please try again later.",
 detail = exception.Message
 };

 var result = JsonSerializer.Serialize(problem);
 context.Response.ContentType = "application/json";
 context.Response.StatusCode = (int)code;
 return context.Response.WriteAsync(result);
 }
}
