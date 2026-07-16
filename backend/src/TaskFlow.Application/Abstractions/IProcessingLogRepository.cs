using TaskFlow.Domain.ProcessingLogs;

namespace TaskFlow.Application.Abstractions;

public interface IProcessingLogRepository
{
    Task AddAsync(ProcessingLog processingLog, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Guid taskId,
        string eventType,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProcessingLog>> ListAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProcessingLog>> ListByTaskIdAsync(
        Guid taskId,
        CancellationToken cancellationToken = default);
}
