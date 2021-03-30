using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {
        protected readonly IWebHostEnvironment _environment;
        public ExceptionMiddleware(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            Log.Error(exception, $"Something went wrong");

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var message = _environment.IsDevelopment() ? exception.ToString() : "Internal Server Error.";

            await WriteError(httpContext, message);
        }

        private async Task WriteError(HttpContext context, string error)
        {
            var message = JsonConvert.SerializeObject(error, _jsonSerializerSettings.Value);
            await context.Response.WriteAsync(message);
        }

        protected static Lazy<JsonSerializerSettings> _jsonSerializerSettings = new Lazy<JsonSerializerSettings>(() =>
        {
            var res = new JsonSerializerSettings();

            res.ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };

            return res;
        });
    }
}