using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Application.Tasks;

public sealed class CreateTaskUseCase
{
    private readonly ITaskRepository taskRepository;
    private readonly IUnitOfWork unitOfWork;

    public CreateTaskUseCase(
        ITaskRepository taskRepository,
        IUnitOfWork unitOfWork)
    {
        this.taskRepository = taskRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<TaskResponse>> ExecuteAsync(
        CreateTaskRequest? request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            return ApplicationResult<TaskResponse>.ValidationFailure(
                new ApplicationError("request.required", "Request body is required."));
        }

        var titleError = TaskRequestValidator.ValidateTitle(request.Title);
        if (titleError is not null)
        {
            return ApplicationResult<TaskResponse>.ValidationFailure(titleError);
        }

        try
        {
            var task = TaskItem.Create(request.Title!, request.Description);

            await taskRepository.AddAsync(task, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return ApplicationResult<TaskResponse>.Success(task.ToResponse());
        }
        catch (DomainValidationException exception)
        {
            return ApplicationResult<TaskResponse>.ValidationFailure(
                new ApplicationError("task.validation", exception.Message));
        }
    }
}
