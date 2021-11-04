using Amazon.DynamoDBv2;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
namespace LbhNotificationsApi
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context).ConfigureAwait(false);
            }
            catch (ArgumentNullException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest).ConfigureAwait(false);
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest).ConfigureAwait(false);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest).ConfigureAwait(false);
            }
            catch (AmazonDynamoDBException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError).ConfigureAwait(false);
            }

#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError).ConfigureAwait(false);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode code)
        {
            _logger.LogError(ex, $"Exception {ex.StackTrace}{Environment.NewLine}{ex.InnerException?.Message}");


            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int) code;
            var allMessageText = ex.GetFullMessage();

            var details = _env.IsDevelopment() && code == HttpStatusCode.InternalServerError
                ? ex.StackTrace :
                  string.Empty;
            await response.WriteAsync(JsonSerializer.Serialize(new BaseErrorResponse((int) code, allMessageText, details)))
                .ConfigureAwait(false);
            //await response.WriteAsync(JsonSerializer.Serialize(new ProblemDetailsException((int) code, $"{allMessageText}{Environment.NewLine}{details}")))
            //        .ConfigureAwait(false);
        }
    }

}
