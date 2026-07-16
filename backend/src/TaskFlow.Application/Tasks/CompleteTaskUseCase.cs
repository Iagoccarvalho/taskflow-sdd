using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;
using TaskFlow.Application.ProcessingLogs;
using TaskFlow.Domain.ProcessingLogs;

namespace TaskFlow.Application.Tasks;

public sealed class CompleteTaskUseCase
{
    private readonly IProcessingLogRepository processingLogRepository;
    private readonly ITaskRepository taskRepository;
    private readonly IUnitOfWork unitOfWork;

    public CompleteTaskUseCase(
        ITaskRepository taskRepository,
        IProcessingLogRepository processingLogRepository,
        IUnitOfWork unitOfWork)
    {
        this.taskRepository = taskRepository;
        this.processingLogRepository = processingLogRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<CompleteTaskResponse>> ExecuteAsync(
        Guid taskId,
        CancellationToken cancellationToken = default)
    {
        if (taskId == Guid.Empty)
        {
            return ApplicationResult<CompleteTaskResponse>.ValidationFailure(
                new ApplicationError("task.id.required", "Task id is required."));
        }

        var task = await taskRepository.GetByIdAsync(taskId, cancellationToken);
        if (task is null)
        {
            return ApplicationResult<CompleteTaskResponse>.NotFound(
                "task.not_found",
                "Task was not found.");
        }

        var completedNow = task.Complete();
        var processingLogCreated = false;

        if (completedNow)
        {
            var completionLogExists = await processingLogRepository.ExistsAsync(
                task.Id,
                ProcessingLogEventType.TaskCompleted,
                cancellationToken);

            if (!completionLogExists)
            {
                await processingLogRepository.AddAsync(
                    ProcessingLog.CreateTaskCompleted(task.Id),
                    cancellationToken);

                processingLogCreated = true;
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        var response = new CompleteTaskResponse(task.ToResponse(), processingLogCreated);
        return ApplicationResult<CompleteTaskResponse>.Success(response);
    }
}
