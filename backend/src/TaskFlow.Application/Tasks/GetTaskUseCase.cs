using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;

namespace TaskFlow.Application.Tasks;

public sealed class GetTaskUseCase
{
    private readonly ITaskRepository taskRepository;

    public GetTaskUseCase(ITaskRepository taskRepository)
    {
        this.taskRepository = taskRepository;
    }

    public async Task<ApplicationResult<TaskResponse>> ExecuteAsync(
        Guid taskId,
        CancellationToken cancellationToken = default)
    {
        if (taskId == Guid.Empty)
        {
            return ApplicationResult<TaskResponse>.ValidationFailure(
                new ApplicationError("task.id.required", "Task id is required."));
        }

        var task = await taskRepository.GetByIdAsync(taskId, cancellationToken);
        if (task is null)
        {
            return ApplicationResult<TaskResponse>.NotFound(
                "task.not_found",
                "Task was not found.");
        }

        return ApplicationResult<TaskResponse>.Success(task.ToResponse());
    }
}
