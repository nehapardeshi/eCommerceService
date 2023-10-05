using eCommerceService.Exceptions;
using eCommerceService.Models;
using Newtonsoft.Json;
using System.Net;

namespace eCommerceService.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ECommerceException eComEx)
            {
                _logger.LogError($"{eComEx.ErrorCode.ErrorCodeName}: {eComEx.Message}");
                await HandleECommerceExceptionAsync(httpContext, eComEx);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleECommerceExceptionAsync(HttpContext context, ECommerceException eComEx)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)eComEx.ErrorCode.StatusCode;
            var responseBody = JsonConvert.SerializeObject(new Error()
            {
                Code = eComEx.ErrorCode.ErrorCodeName,
                Message = eComEx.Message
            });
            await context.Response.WriteAsync(responseBody);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var responseBody = JsonConvert.SerializeObject(new Error()
            {
                Code = "InternalServerError",
                Message = exception.Message
            });
            await context.Response.WriteAsync(responseBody);
        }
    }
}
