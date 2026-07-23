using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Contracts;
using TaskFlow.Application.Common;

namespace TaskFlow.Api.Controllers;

internal static class ControllerResultExtensions
{
    public static bool TryParseTaskId(
        this ControllerBase controller,
        string id,
        out Guid taskId,
        out ActionResult? errorResult)
    {
        if (!Guid.TryParse(id, out taskId) || taskId == Guid.Empty)
        {
            errorResult = controller.BadRequest(new ErrorResponse(
                "Invalid task id.",
                new[] { "Task id must be a non-empty UUID." }));

            return false;
        }

        errorResult = null;
        return true;
    }

    public static ActionResult ToErrorResult<T>(
        this ControllerBase controller,
        ApplicationResult<T> result)
    {
        var error = result.Errors.FirstOrDefault()?.Message ?? "Request could not be processed.";
        var details = result.Errors.Select(applicationError => applicationError.Message).ToList();
        var response = new ErrorResponse(error, details);

        return result.ErrorType switch
        {
            ApplicationErrorType.Validation => controller.BadRequest(response),
            ApplicationErrorType.NotFound => controller.NotFound(response),
            _ => controller.StatusCode(StatusCodes.Status500InternalServerError, response)
        };
    }
}
