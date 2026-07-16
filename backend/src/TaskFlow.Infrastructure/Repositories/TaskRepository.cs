using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Tasks;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.Repositories;

public sealed class TaskRepository : ITaskRepository
{
    private readonly TaskFlowDbContext dbContext;

    public TaskRepository(TaskFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        await dbContext.Tasks.AddAsync(task, cancellationToken);
    }

    public async Task<TaskItem?> GetByIdAsync(
        Guid taskId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Tasks
            .SingleOrDefaultAsync(task => task.Id == taskId, cancellationToken);
    }

    public async Task<IReadOnlyList<TaskItem>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Tasks
            .AsNoTracking()
            .OrderByDescending(task => task.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
