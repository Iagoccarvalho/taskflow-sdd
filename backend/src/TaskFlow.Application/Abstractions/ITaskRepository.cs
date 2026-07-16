using TaskFlow.Domain.Tasks;

namespace TaskFlow.Application.Abstractions;

public interface ITaskRepository
{
    Task AddAsync(TaskItem task, CancellationToken cancellationToken = default);

    Task<TaskItem?> GetByIdAsync(Guid taskId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TaskItem>> ListAsync(CancellationToken cancellationToken = default);
}
