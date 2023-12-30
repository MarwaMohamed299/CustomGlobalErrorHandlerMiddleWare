
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace GlobalErrorHandling.ErrorHandlerMiddleWare
{
    public class GlobalErrorHandlerMiddleWare : IMiddleware
    {
        private readonly ILogger<GlobalErrorHandlerMiddleWare> _logger;

        public GlobalErrorHandlerMiddleWare(ILogger<GlobalErrorHandlerMiddleWare>logger)
        {
           _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
               await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ProblemDetails problems = new()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server Error",

                };
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(problems);
            }
        }
    }
}
