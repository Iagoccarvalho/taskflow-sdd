using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;
using TaskFlow.Domain.Common;

namespace TaskFlow.Application.Tasks;

public sealed class UpdateTaskUseCase
{
    private readonly ITaskRepository taskRepository;
    private readonly IUnitOfWork unitOfWork;

    public UpdateTaskUseCase(
        ITaskRepository taskRepository,
        IUnitOfWork unitOfWork)
    {
        this.taskRepository = taskRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<TaskResponse>> ExecuteAsync(
        Guid taskId,
        UpdateTaskRequest? request,
        CancellationToken cancellationToken = default)
    {
        var errors = Validate(taskId, request);
        if (errors.Count > 0)
        {
            return ApplicationResult<TaskResponse>.ValidationFailure(errors.ToArray());
        }

        var task = await taskRepository.GetByIdAsync(taskId, cancellationToken);
        if (task is null)
        {
            return ApplicationResult<TaskResponse>.NotFound(
                "task.not_found",
                "Task was not found.");
        }

        try
        {
            task.UpdateDetails(request!.Title!, request.Description);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return ApplicationResult<TaskResponse>.Success(task.ToResponse());
        }
        catch (DomainValidationException exception)
        {
            return ApplicationResult<TaskResponse>.ValidationFailure(
                new ApplicationError("task.validation", exception.Message));
        }
    }

    private static IReadOnlyList<ApplicationError> Validate(Guid taskId, UpdateTaskRequest? request)
    {
        var errors = new List<ApplicationError>();

        if (taskId == Guid.Empty)
        {
            errors.Add(new ApplicationError("task.id.required", "Task id is required."));
        }

        if (request is null)
        {
            errors.Add(new ApplicationError("request.required", "Request body is required."));
            return errors;
        }

        var titleError = TaskRequestValidator.ValidateTitle(request.Title);
        if (titleError is not null)
        {
            errors.Add(titleError);
        }

        return errors;
    }
}
