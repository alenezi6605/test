﻿using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TestsService.AppCore.Infrastructure.Validator;
using TestsService.Domain.Common;
using TestsService.Domain.Exceptions;
using Microsoft.Extensions.Localization;

namespace TestsService.API.Utility
{

    public class JsonExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly Func<object, Task> _clearCacheHeadersDelegate;
        private readonly JsonSerializer _serializer;
        private readonly IStringLocalizer<Localization> _localizer;

        public JsonExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IStringLocalizer<Localization> localizer)
        {
            _next = next;
            _localizer = localizer;

            _logger = loggerFactory.CreateLogger<Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware>();
            _clearCacheHeadersDelegate = ClearCacheHeaders;

            _serializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Exception middlewareError)
            {
                _logger.LogError(middlewareError, _localizer["UnHandledException"]);

                if (context.Response.HasStarted)
                {
                    _logger.LogWarning(_localizer["LogWarning"]);
                    throw;
                }

                try
                {
                    // reset body
                    if (context.Response.Body.CanSeek)
                        context.Response.Body.SetLength(0L);

                    context.Response.StatusCode = 500;
                    context.Response.OnStarting(_clearCacheHeadersDelegate, context.Response);

                    await WriteContent(context, middlewareError).ConfigureAwait(false);

                    return;
                }
                catch (Exception handlerError)
                {
                    // Suppress secondary exceptions, re-throw the original.
                    _logger.LogError(handlerError, _localizer["HandlerError"]);
                }

                throw; // Re-throw the original if we couldn't handle it
            }
        }

        private async Task WriteContent(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            switch (exception)
            {
                case ValidationException validationException:
                    var errors = validationException.ValidationResultModel;
                    context.Response.StatusCode = errors.StatusCode;
                    var serializerSettings = new JsonSerializerSettings();
                    serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    string result = JsonConvert.SerializeObject(errors);
                    await context.Response.WriteAsync(result);
                    break;
                case DomainException domainException:
                    context.Response.StatusCode = domainException.StatusCode;
                    await ResponseModel(domainException, context);
                    break;
                default:
                    await ResponseModel(exception, context);
                    break;
            }

        }

        private async Task ResponseModel(Exception exception, HttpContext context)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            string result = JsonConvert.SerializeObject(new ErrorResponseModel()
            {
                Message = exception.Message,
            }, serializerSettings);
            await context.Response.WriteAsync(result);
        }
        private async Task ResponseModel(DomainException domainException, HttpContext context)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            string result = JsonConvert.SerializeObject(new ErrorResponseModel()
            {
                Message = domainException.Message,
            }, serializerSettings);
            await context.Response.WriteAsync(result);
        }

        private Task ClearCacheHeaders(object state)
        {
            if (state is not HttpResponse response)
                return Task.CompletedTask;

            response.Headers[HeaderNames.CacheControl] = "no-cache";
            response.Headers[HeaderNames.Pragma] = "no-cache";
            response.Headers[HeaderNames.Expires] = "-1";
            response.Headers.Remove(HeaderNames.ETag);

            return Task.CompletedTask;
        }

    }

}