using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Api.Models.Common;
using Api.Models.Enums;
using Core.Exceptions;

// /*
// Added to suppress the following warning :
// Warning : Add a public read-only property accessor for positional argument logger of Attribute CustomExceptionFilter
// */
// #pragma warning disable CA1019

namespace Api.Filters;

public class CustomExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger<CustomExceptionFilter> _logger;

    public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
    {
        _logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
        BaseResponse<int> response = new BaseResponse<int>(ResponseStatus.Error);
        int statusCode;

        if (context.Exception is UnauthorizedAccessException)
        {
            statusCode = StatusCodes.Status401Unauthorized;
            response.Message = context.Exception.Message;
        }
        else if (context.Exception is NotFoundException)
        {
            statusCode = StatusCodes.Status404NotFound;
            response.Message = context.Exception.Message;
        }
        else if (context.Exception is BadRequestException)
        {
            statusCode = StatusCodes.Status400BadRequest;
            response.Message = context.Exception.Message;
        }
        else if (context.Exception is AlreadyExistsException)
        {
            statusCode = StatusCodes.Status409Conflict;
            response.Message = context.Exception.Message;
        }
        else
        {
            _logger.LogError($"Internal server exception: {context.Exception.ToString()}");

            statusCode = StatusCodes.Status500InternalServerError;
            response.Message = $"An internal server error has occurred: {context.Exception.Message}";
        }

        context.Result = new JsonResult(response)
        {
            StatusCode = statusCode
        };
    }
}
