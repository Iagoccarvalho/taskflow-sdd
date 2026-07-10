using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Tasks;

public sealed class TaskItem
{
    private TaskItem(
        Guid id,
        string title,
        string? description,
        TaskItemStatus status,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt,
        DateTimeOffset? completedAt)
    {
        Id = id;
        Title = title;
        Description = description;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        CompletedAt = completedAt;
    }

    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public string? Description { get; private set; }

    public TaskItemStatus Status { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public DateTimeOffset? CompletedAt { get; private set; }

    public static TaskItem Create(string title, string? description = null)
    {
        var now = DateTimeOffset.UtcNow;

        return new TaskItem(
            Guid.NewGuid(),
            ValidateTitle(title),
            description,
            TaskItemStatus.Pending,
            now,
            now,
            completedAt: null);
    }

    public void UpdateDetails(string title, string? description)
    {
        Title = ValidateTitle(title);
        Description = description;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public bool Complete()
    {
        if (Status == TaskItemStatus.Completed)
        {
            return false;
        }

        var now = DateTimeOffset.UtcNow;

        Status = TaskItemStatus.Completed;
        UpdatedAt = now;
        CompletedAt = now;

        return true;
    }

    private static string ValidateTitle(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainValidationException("Task title is required.");
        }

        return title;
    }
}
