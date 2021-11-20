using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sc.DevChallenge.Application.Common.Exceptions.ApiException;
using Sc.DevChallenge.Application.Models;
using Sc.DevChallenge.Application.Models.ResultModels;

namespace SC.DevChallenge.Api.Utils.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;
        
        private const string DefaultStatus = "Internal Server Error";
        private const HttpStatusCode DefaultStatusCode = HttpStatusCode.InternalServerError;
        private const string DefaultErrorMessage = "The problem is on our side";

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            _logger.LogError($"Message: {exception.Message}\n Stack trace: {exception.StackTrace}");

            var code = DefaultStatusCode;
            var status = DefaultStatus;
            var message = DefaultErrorMessage;

            IDictionary<string, string[]> errors = null;

            if (exception is BaseApiException apiException)
            {
                // Add validation failures if ValidationException
                if (apiException is BadRequestException validationException) errors = validationException.Failures;

                // BaseApiException
                status = apiException.Status;
                message = apiException.Message;
                code = apiException.StatusCode;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var errorModel = new BaseErrorModel
            {
                Status = status,
                Message = message,
                Errors = errors
            };

            var result = JsonConvert.SerializeObject(errorModel);

            return context.Response.WriteAsync(result);
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}