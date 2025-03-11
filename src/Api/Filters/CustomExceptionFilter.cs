using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Api.Models.Common;
using Api.Models.Enums;
using Core.Exceptions;
using Microsoft.AspNetCore.Http;

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
        
        (int statusCode, string message) = context.Exception switch
        {
            UnauthorizedAccessException ex => (StatusCodes.Status401Unauthorized, ex.Message),
            NotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
            BadRequestException ex => (StatusCodes.Status400BadRequest, ex.Message),
            AlreadyExistsException ex => (StatusCodes.Status409Conflict, ex.Message),
            _ => HandleUnexpectedException(context.Exception)
        };
        response.Message = message;
        context.Result = new JsonResult(response)
        {
            StatusCode = statusCode
        };
    }

    private (int StatusCode, string Message) HandleUnexpectedException(Exception ex)
    {
        _logger.LogError(ex, "An unexpected error occurred");
        return (StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
    }
}
