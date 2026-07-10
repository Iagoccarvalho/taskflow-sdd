using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.ProcessingLogs;

public sealed class ProcessingLog
{
    private ProcessingLog(
        Guid id,
        Guid taskId,
        string eventType,
        string message,
        DateTimeOffset createdAt)
    {
        Id = id;
        TaskId = taskId;
        EventType = eventType;
        Message = message;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }

    public Guid TaskId { get; private set; }

    public string EventType { get; private set; }

    public string Message { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public static ProcessingLog CreateTaskCompleted(Guid taskId)
    {
        if (taskId == Guid.Empty)
        {
            throw new DomainValidationException("Task id is required.");
        }

        return new ProcessingLog(
            Guid.NewGuid(),
            taskId,
            ProcessingLogEventType.TaskCompleted,
            "Task completion was processed.",
            DateTimeOffset.UtcNow);
    }
}
