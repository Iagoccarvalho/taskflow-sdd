using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.Contracts;

public sealed record CreateTaskRequest(
    [Required] string? Title,
    string? Description);

public sealed record UpdateTaskRequest(
    [Required] string? Title,
    string? Description);

public sealed record TaskResponse(
    Guid Id,
    string Title,
    string? Description,
    string Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    DateTimeOffset? CompletedAt);

public sealed record CompleteTaskResponse(
    TaskResponse Task,
    bool ProcessingLogCreated);
