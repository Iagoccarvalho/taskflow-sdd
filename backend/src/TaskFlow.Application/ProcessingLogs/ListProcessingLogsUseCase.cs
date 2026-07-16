using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;

namespace TaskFlow.Application.ProcessingLogs;

public sealed class ListProcessingLogsUseCase
{
    private readonly IProcessingLogRepository processingLogRepository;

    public ListProcessingLogsUseCase(IProcessingLogRepository processingLogRepository)
    {
        this.processingLogRepository = processingLogRepository;
    }

    public async Task<ApplicationResult<IReadOnlyList<ProcessingLogResponse>>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        var logs = await processingLogRepository.ListAsync(cancellationToken);
        var response = logs.Select(log => log.ToResponse()).ToList();

        return ApplicationResult<IReadOnlyList<ProcessingLogResponse>>.Success(response);
    }
}
