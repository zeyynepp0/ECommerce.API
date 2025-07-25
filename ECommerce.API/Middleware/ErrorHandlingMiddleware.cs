using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
//using FluentValidation;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Normal istek akışına devam et
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex); // Hata varsa yakala
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = HttpStatusCode.InternalServerError;
            object errorResponse;

            switch (exception)
            {
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    errorResponse = new
                    {
                        status = false,
                        message = "Yetkisiz erişim. Lütfen giriş yapın."
                    };
                    break;

                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    errorResponse = new
                    {
                        status = false,
                        message = "İstenilen kaynak bulunamadı."
                    };
                    break;

                //case ValidationException validationEx:
                //    statusCode = HttpStatusCode.BadRequest;
                //    errorResponse = new
                //    {
                //        status = false,
                //        message = "Validasyon hatası oluştu.",
                //        errors = validationEx.Errors.Select(e => new
                //        {
                //            field = e.PropertyName,
                //            error = e.ErrorMessage
                //        })
                //    };
                //    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    errorResponse = new
                    {
                        status = false,
                        message = "Beklenmeyen bir hata oluştu.",
                        error = exception.Message
                    };
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await context.Response.WriteAsync(json);
        }
    }
}
