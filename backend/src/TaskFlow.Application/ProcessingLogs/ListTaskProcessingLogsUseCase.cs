using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;

namespace TaskFlow.Application.ProcessingLogs;

public sealed class ListTaskProcessingLogsUseCase
{
    private readonly IProcessingLogRepository processingLogRepository;
    private readonly ITaskRepository taskRepository;

    public ListTaskProcessingLogsUseCase(
        ITaskRepository taskRepository,
        IProcessingLogRepository processingLogRepository)
    {
        this.taskRepository = taskRepository;
        this.processingLogRepository = processingLogRepository;
    }

    public async Task<ApplicationResult<IReadOnlyList<ProcessingLogResponse>>> ExecuteAsync(
        Guid taskId,
        CancellationToken cancellationToken = default)
    {
        if (taskId == Guid.Empty)
        {
            return ApplicationResult<IReadOnlyList<ProcessingLogResponse>>.ValidationFailure(
                new ApplicationError("task.id.required", "Task id is required."));
        }

        var task = await taskRepository.GetByIdAsync(taskId, cancellationToken);
        if (task is null)
        {
            return ApplicationResult<IReadOnlyList<ProcessingLogResponse>>.NotFound(
                "task.not_found",
                "Task was not found.");
        }

        var logs = await processingLogRepository.ListByTaskIdAsync(taskId, cancellationToken);
        var response = logs.Select(log => log.ToResponse()).ToList();

        return ApplicationResult<IReadOnlyList<ProcessingLogResponse>>.Success(response);
    }
}
