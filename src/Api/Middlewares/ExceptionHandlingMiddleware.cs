using Microsoft.AspNetCore.Mvc.ModelBinding;
using Api.Models.Common;
using Api.Models.Enums;
using Core.Exceptions;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context).ConfigureAwait(false);
        }
        catch (UnauthorizedAccessException exception)
        {
            await HandleExceptionAsync(context, StatusCodes.Status401Unauthorized, exception.Message).ConfigureAwait(false);
        }
        catch (NotFoundException exception)
        {
            await HandleExceptionAsync(context, StatusCodes.Status404NotFound, exception.Message).ConfigureAwait(false);
        }
        catch (BadRequestException exception)
        {
            await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, exception.Message).ConfigureAwait(false);
        }
        catch (BadHttpRequestException exception)
        {
            ModelStateDictionary? modelState = null;

            // Check if ModelState is stored in HttpContext.Items
            if (context.Items.ContainsKey("ModelState"))
            {
                modelState = context.Items["ModelState"] as ModelStateDictionary;
            }

            BaseResponse<int> response = new(ResponseStatus.Error)
            {
                Message = exception.Message,
                Errors = modelState?
                    .Where(modelError => modelError.Value != null && modelError.Value.Errors.Any())
                    .ToDictionary(
                        modelError => modelError.Key,
                        modelError => modelError.Value != null
                            ? modelError.Value.Errors.Select(e => e.ErrorMessage).ToList()
                            : new List<string>()
                    )
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            JsonSerializerOptions options = SendJsonResponse();

            await context.Response.WriteAsJsonAsync(response, options).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            _logger.LogError($"Internal server exception: {exception.Message}");
            _logger.LogError($"Inner exception: {exception.InnerException}");
            _logger.LogError($"Exception stack trace: {exception.StackTrace}");

            string message = $"An internal server error has occurred: {exception.Message}";

            await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, message).ConfigureAwait(false);
        }
    }

    private static JsonSerializerOptions SendJsonResponse()
    {
        return new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    private async Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
    {
        BaseResponse<int> response = new BaseResponse<int>(ResponseStatus.Error)
        {
            Message = message
        };

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(response, _jsonOptions).ConfigureAwait(false);
    }
}
