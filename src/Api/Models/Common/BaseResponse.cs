using Microsoft.AspNetCore.Mvc;
using Api.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Models.Common;

/// <summary>
/// Represents the base response structure for API responses.
/// </summary>
/// <typeparam name="T">The type of the data in the response.</typeparam>
public class BaseResponse<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseResponse{T}"/> class with a specific status.
    /// </summary>
    /// <param name="status">The response status.</param>
    public BaseResponse(ResponseStatus status) => Status = status;

    /// <summary>
    /// Gets or sets the status of the response.
    /// </summary>
    public ResponseStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the data returned in the response.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets the message associated with the response.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the stack trace information, if any.
    /// </summary>
    public string? StackTrace { get; set; }

    /// <summary>
    /// Gets or sets the collection of errors associated with the response.
    /// </summary>
    public Dictionary<string, List<string>>? Errors { get; set; }

    /// <summary>
    /// Gets or sets pagination metadata for the response, if applicable.
    /// </summary>
    public PaginationMetadata? Pagination { get; set; }
}

/// <summary>
/// Provides utilities for generating bad request responses based on model validation errors.
/// </summary>
public class ModelValidationBadRequest
{
    /// <summary>
    /// Generates a standardized BadRequest response with model validation errors.
    /// </summary>
    /// <param name="modelState">The model state containing validation errors.</param>
    /// <returns>A BadRequestObjectResult encapsulating the validation errors.</returns>
    public static BadRequestObjectResult GenerateErrorResponse(ModelStateDictionary modelState)
    {
        return new BadRequestObjectResult(new BaseResponse<int>(ResponseStatus.Error)
        {
            Errors = modelState
                .Where(entry => entry.Value != null && entry.Value.Errors.Any())
                .ToDictionary(
                    entry => entry.Key,
                    entry => entry.Value!.Errors.Select(error => error.ErrorMessage).ToList()
                )
        });
    }
}

/// <summary>
/// Provides utilities for generating bad request responses based on model validation errors.
/// </summary>
public class PaginationMetadata
{
    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total count of items (if available).
    /// </summary>
    public int? TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the cursor for the previous page.
    /// </summary>
    public DateTime? PrevCursor { get; set; }

    /// <summary>
    /// Gets or sets the cursor for the next page.
    /// </summary>
    public DateTime? NextCursor { get; set; }
}