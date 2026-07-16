using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;

namespace TaskFlow.Application.Tasks;

public sealed class ListTasksUseCase
{
    private readonly ITaskRepository taskRepository;

    public ListTasksUseCase(ITaskRepository taskRepository)
    {
        this.taskRepository = taskRepository;
    }

    public async Task<ApplicationResult<IReadOnlyList<TaskResponse>>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        var tasks = await taskRepository.ListAsync(cancellationToken);
        var response = tasks.Select(task => task.ToResponse()).ToList();

        return ApplicationResult<IReadOnlyList<TaskResponse>>.Success(response);
    }
}
