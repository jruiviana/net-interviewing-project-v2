using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Insurance.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Insurance.Api.MiddleWares
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }
        
        public async Task Invoke(HttpContext context)
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
        
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode code;
            var message = exception.Message;

            switch (exception)
            {
                case AuthorizationException _:
                    code = HttpStatusCode.Forbidden;
                    message = $"Forbidden: {context.Request.Path + context.Request.QueryString}.";
                    _logger.LogWarning(exception, message);
                    break;
                case BadRequestException _:
                    code = HttpStatusCode.BadRequest;
                    message = $"Bad Request: {context.Request.Method} {context.Request.Path + context.Request.QueryString}.";
                    _logger.LogWarning(exception, message);
                    break;
                case ResourceNotFoundException _:
                    code = HttpStatusCode.NotFound;
                    message = $"Not Found: {context.Request.Path + context.Request.QueryString}.";
                    _logger.LogWarning(exception, message);
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    message = "Something went wrong during request handling";
                    _logger.LogError(exception, $"Error: {exception.Message}.");
                    break;
            }

            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsync(message);
        }
    }
}