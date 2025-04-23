using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Api.Models.Common;
using Api.Models.Enums;
using Core.Exceptions;
using Microsoft.AspNetCore.Http;

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
        BaseResponse<int> response = new BaseResponse<int>(ResponseStatus.Fail);

        (int statusCode, string message, ResponseStatus status) = context.Exception switch
        {
            UnauthorizedAccessException ex => (StatusCodes.Status401Unauthorized, ex.Message, ResponseStatus.Fail),
            NotFoundException ex => (StatusCodes.Status404NotFound, ex.Message, ResponseStatus.Fail),
            BadRequestException ex => (StatusCodes.Status400BadRequest, ex.Message, ResponseStatus.Fail),
            AlreadyExistsException ex => (StatusCodes.Status409Conflict, ex.Message, ResponseStatus.Fail),
            DatabaseOperationException ex => (StatusCodes.Status500InternalServerError, ex.Message, ResponseStatus.Error),
            _ => HandleUnexpectedException(context.Exception)
        };
        response.Status = status;
        response.Message = message;
        context.Result = new JsonResult(response)
        {
            StatusCode = statusCode
        };
    }

    private (int StatusCode, string Message, ResponseStatus status) HandleUnexpectedException(Exception ex)
    {
        _logger.LogError(ex, "An unexpected error occurred");
        return (StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.", ResponseStatus.Error);
    }
}
