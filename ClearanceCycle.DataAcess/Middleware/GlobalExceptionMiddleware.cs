using ClearanceCycle.Application.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace ClearanceCycle.DataAcess.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly Microsoft.AspNetCore.Http.RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger, Microsoft.AspNetCore.Http.RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(Microsoft.AspNetCore.Http.HttpContext context)
        {
            var originalStream = context.Response.Body;
            var stream = new MemoryStream();
            context.Response.Body = stream;
            try
            {
                // Log request details
                var request = context.Request;
                var requestBody = await ReadRequestBodyAsync(request);
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                await _next(context);

                stopwatch.Stop();

                // Read the response body
                stream.Seek(0, SeekOrigin.Begin);
                var responseBody = new StreamReader(stream).ReadToEnd();
                stream.Seek(0, SeekOrigin.Begin);

                // Copy the content back to the original response stream
                await stream.CopyToAsync(originalStream);

                // Log response details
                var logObject = new
                {
                    Request = requestBody,
                    Time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffff"),
                    URL = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}",
                    Method = context.Request.Method,
                    Headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                    Response = responseBody,
                    RequestTime = stopwatch.Elapsed.TotalSeconds,
                };

                _logger.LogInformation("{LogInfo}", logObject);

            }
            catch(InvalidOperationException ex)
            {
                await HandleException(ex, context, originalStream,ex.Message);

            }
            catch (Exception ex)
            {
                await HandleException(ex, context, originalStream, "An unexpected error occurred.");
            }
            finally
            {
                context.Response.Body = originalStream;
                stream.Dispose();
            }
        }


        private async Task HandleException(Exception exception, Microsoft.AspNetCore.Http.HttpContext context, Stream originalStream,string message)
        {
            _logger.LogError(exception, "An exception was thrown: {ExceptionType}", exception.GetType().Name);
            context.Response.Body = originalStream;

            context.Response.ContentType = "application/json";

            var responseModel = HandleResponse(message);
            var result = System.Text.Json.JsonSerializer.Serialize(responseModel);
            await context.Response.WriteAsync(result);

        }

        private ReponseDto HandleResponse(string message)
        {
            return new ReponseDto
            {
                Success = false,
                Message = message,

            };
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin); 
            return body;
        }
    }

    public static class MiddleWareExtensions
    {
        public static IApplicationBuilder HandleRequestMiddleware(this IApplicationBuilder application)
        {
            return application.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}

