using EnrollmentSystem.API.Contracts;
using EnrollmentSystem.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EnrollmentSystem.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (context.Request.ContentLength == 0 && context.Request.Body.CanRead)
                {
                    throw new BadRequestException();
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                HandleException(context, ex);
            }
        }

        private void HandleException(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "\n\n Exception occured in the middleware.\n\n");
            context.Response.ContentType = "application/json";

            int statusCode;
            string message;

            if (ex is NotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                message = "Not Found!";

            }
            else if (ex is BadRequestException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                message = "Bad Request!";
            }
            else
            {
                _logger.LogError(ex, "\n\n Unhandled exception in the middleware.\n\n");
                statusCode = (int)HttpStatusCode.InternalServerError;
                message = "Internal server Error!";
            }

            context.Response.StatusCode = statusCode;

            var error = new Error
            {
                StatusCode = statusCode.ToString(),
                Message = message 
            };

            context.Response.WriteAsync(error.ToString());
        }
    }
}
