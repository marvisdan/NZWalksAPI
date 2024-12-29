using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Net;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging.Abstractions;


namespace NZWalksAPI.Middlewares
{
    public class ExceptionhandlerMiddleware
    {
        private readonly ILogger<ExceptionhandlerMiddleware> logger;
        private readonly RequestDelegate next;
        private readonly IHostEnvironment env;

        public ExceptionhandlerMiddleware(ILogger<ExceptionhandlerMiddleware> logger, RequestDelegate next, IHostEnvironment env)
        {
            this.logger = logger;
            this.env = env;
            this.next = next;

        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                //Log the exception
                logger.LogError(ex, $"{errorId} : {ex.Message}");

                //Return a custom error response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";
                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong. We are looking into it."
                };
                var jsonError = JsonSerializer.Serialize(error);
                await httpContext.Response.WriteAsync(jsonError);
            }
        }
    }
}