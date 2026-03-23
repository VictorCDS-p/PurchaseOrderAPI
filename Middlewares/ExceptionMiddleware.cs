using System.Net;
using System.Text.Json;

namespace PurchaseOrderAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;

            if (exception.Message.Contains("não encontrado"))
                statusCode = HttpStatusCode.NotFound;

            else if (exception.Message.Contains("obrigatório") ||
                     exception.Message.Contains("inválido") ||
                     exception.Message.Contains("já cadastrado"))
                statusCode = HttpStatusCode.BadRequest;

            var response = new
            {
                error = exception.GetType().Name,
                message = exception.Message,
                status = (int)statusCode
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}