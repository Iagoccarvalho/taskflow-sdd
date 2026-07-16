using TaskFlow.Domain.ProcessingLogs;

namespace TaskFlow.Application.ProcessingLogs;

public static class ProcessingLogMappings
{
    public static ProcessingLogResponse ToResponse(this ProcessingLog processingLog)
    {
        return new ProcessingLogResponse(
            processingLog.Id,
            processingLog.TaskId,
            processingLog.EventType,
            processingLog.Message,
            processingLog.CreatedAt);
    }
}
