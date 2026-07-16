using TaskFlow.Application.Tasks;

namespace TaskFlow.Application.ProcessingLogs;

public sealed record ProcessingLogResponse(
    Guid Id,
    Guid TaskId,
    string EventType,
    string Message,
    DateTimeOffset CreatedAt);

public sealed record CompleteTaskResponse(
    TaskResponse Task,
    bool ProcessingLogCreated);
