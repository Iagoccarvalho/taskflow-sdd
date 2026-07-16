namespace TaskFlow.Application.Tasks;

public sealed record CreateTaskRequest(string? Title, string? Description);

public sealed record UpdateTaskRequest(string? Title, string? Description);

public sealed record TaskResponse(
    Guid Id,
    string Title,
    string? Description,
    string Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    DateTimeOffset? CompletedAt);
