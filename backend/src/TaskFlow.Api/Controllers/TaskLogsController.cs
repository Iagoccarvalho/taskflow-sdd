using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Contracts;
using TaskFlow.Application.ProcessingLogs;
using ApiProcessingLogResponse = TaskFlow.Api.Contracts.ProcessingLogResponse;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/tasks/{id}/logs")]
public sealed class TaskLogsController : ControllerBase
{
    private readonly ListTaskProcessingLogsUseCase listTaskProcessingLogsUseCase;

    public TaskLogsController(ListTaskProcessingLogsUseCase listTaskProcessingLogsUseCase)
    {
        this.listTaskProcessingLogsUseCase = listTaskProcessingLogsUseCase;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ApiProcessingLogResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<ApiProcessingLogResponse>>> ListByTaskAsync(
        string id,
        CancellationToken cancellationToken)
    {
        if (!this.TryParseTaskId(id, out var taskId, out var errorResult))
        {
            return errorResult!;
        }

        var result = await listTaskProcessingLogsUseCase.ExecuteAsync(taskId, cancellationToken);
        if (!result.Succeeded)
        {
            return this.ToErrorResult(result);
        }

        var response = result.Value!
            .Select(log => log.ToApiResponse())
            .ToList();

        return Ok(response);
    }
}
