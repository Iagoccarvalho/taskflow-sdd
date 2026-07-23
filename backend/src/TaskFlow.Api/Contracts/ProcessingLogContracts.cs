namespace TaskFlow.Api.Contracts;

public sealed record ProcessingLogResponse(
    Guid Id,
    Guid TaskId,
    string EventType,
    string Message,
    DateTimeOffset CreatedAt);
