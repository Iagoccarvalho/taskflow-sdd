using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.ProcessingLogs;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.Repositories;

public sealed class ProcessingLogRepository : IProcessingLogRepository
{
    private readonly TaskFlowDbContext dbContext;

    public ProcessingLogRepository(TaskFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddAsync(
        ProcessingLog processingLog,
        CancellationToken cancellationToken = default)
    {
        await dbContext.ProcessingLogs.AddAsync(processingLog, cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Guid taskId,
        string eventType,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.ProcessingLogs
            .AnyAsync(
                log => log.TaskId == taskId && log.EventType == eventType,
                cancellationToken);
    }

    public async Task<IReadOnlyList<ProcessingLog>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.ProcessingLogs
            .AsNoTracking()
            .OrderByDescending(log => log.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProcessingLog>> ListByTaskIdAsync(
        Guid taskId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.ProcessingLogs
            .AsNoTracking()
            .Where(log => log.TaskId == taskId)
            .OrderByDescending(log => log.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
