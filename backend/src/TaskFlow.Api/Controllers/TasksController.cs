using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Contracts;
using TaskFlow.Application.Tasks;
using ApiCreateTaskRequest = TaskFlow.Api.Contracts.CreateTaskRequest;
using ApiTaskResponse = TaskFlow.Api.Contracts.TaskResponse;
using ApiUpdateTaskRequest = TaskFlow.Api.Contracts.UpdateTaskRequest;
using ApplicationCreateTaskRequest = TaskFlow.Application.Tasks.CreateTaskRequest;
using ApplicationUpdateTaskRequest = TaskFlow.Application.Tasks.UpdateTaskRequest;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public sealed class TasksController : ControllerBase
{
    private const string GetTaskByIdRouteName = "GetTaskById";

    private readonly CompleteTaskUseCase completeTaskUseCase;
    private readonly CreateTaskUseCase createTaskUseCase;
    private readonly GetTaskUseCase getTaskUseCase;
    private readonly ListTasksUseCase listTasksUseCase;
    private readonly UpdateTaskUseCase updateTaskUseCase;

    public TasksController(
        CreateTaskUseCase createTaskUseCase,
        ListTasksUseCase listTasksUseCase,
        GetTaskUseCase getTaskUseCase,
        UpdateTaskUseCase updateTaskUseCase,
        CompleteTaskUseCase completeTaskUseCase)
    {
        this.createTaskUseCase = createTaskUseCase;
        this.listTasksUseCase = listTasksUseCase;
        this.getTaskUseCase = getTaskUseCase;
        this.updateTaskUseCase = updateTaskUseCase;
        this.completeTaskUseCase = completeTaskUseCase;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiTaskResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiTaskResponse>> CreateAsync(
        [FromBody] ApiCreateTaskRequest request,
        CancellationToken cancellationToken)
    {
        var result = await createTaskUseCase.ExecuteAsync(
            new ApplicationCreateTaskRequest(request.Title, request.Description),
            cancellationToken);

        if (!result.Succeeded)
        {
            return this.ToErrorResult(result);
        }

        var response = result.Value!.ToApiResponse();
        return CreatedAtRoute(GetTaskByIdRouteName, new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ApiTaskResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ApiTaskResponse>>> ListAsync(
        CancellationToken cancellationToken)
    {
        var result = await listTasksUseCase.ExecuteAsync(cancellationToken);
        var response = result.Value!
            .Select(task => task.ToApiResponse())
            .ToList();

        return Ok(response);
    }

    [HttpGet("{id}", Name = GetTaskByIdRouteName)]
    [ProducesResponseType(typeof(ApiTaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiTaskResponse>> GetByIdAsync(
        string id,
        CancellationToken cancellationToken)
    {
        if (!this.TryParseTaskId(id, out var taskId, out var errorResult))
        {
            return errorResult!;
        }

        var result = await getTaskUseCase.ExecuteAsync(taskId, cancellationToken);
        if (!result.Succeeded)
        {
            return this.ToErrorResult(result);
        }

        return Ok(result.Value!.ToApiResponse());
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiTaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiTaskResponse>> UpdateAsync(
        string id,
        [FromBody] ApiUpdateTaskRequest request,
        CancellationToken cancellationToken)
    {
        if (!this.TryParseTaskId(id, out var taskId, out var errorResult))
        {
            return errorResult!;
        }

        var result = await updateTaskUseCase.ExecuteAsync(
            taskId,
            new ApplicationUpdateTaskRequest(request.Title, request.Description),
            cancellationToken);

        if (!result.Succeeded)
        {
            return this.ToErrorResult(result);
        }

        return Ok(result.Value!.ToApiResponse());
    }

    [HttpPatch("{id}/complete")]
    [ProducesResponseType(typeof(CompleteTaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CompleteTaskResponse>> CompleteAsync(
        string id,
        CancellationToken cancellationToken)
    {
        if (!this.TryParseTaskId(id, out var taskId, out var errorResult))
        {
            return errorResult!;
        }

        var result = await completeTaskUseCase.ExecuteAsync(taskId, cancellationToken);
        if (!result.Succeeded)
        {
            return this.ToErrorResult(result);
        }

        return Ok(result.Value!.ToApiResponse());
    }
}
