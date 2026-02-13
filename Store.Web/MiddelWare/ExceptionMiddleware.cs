using Store.Service.HandelResponses;
using System.Net;
using System.Text.Json;

namespace Store.Web.MiddelWares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IHostEnvironment _environment;
        public ExceptionMiddleware(RequestDelegate next ,
            IHostEnvironment environment,
            ILogger<ExceptionMiddleware> logger) 
        {
        _next = next;
        _environment = environment;
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
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = _environment.IsDevelopment()
                    ? new CustomException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                    : new CustomException((int)HttpStatusCode.InternalServerError);
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
