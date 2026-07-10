using TaskFlow.Domain.Common;
using TaskFlow.Domain.ProcessingLogs;

namespace TaskFlow.Tests.Domain;

public sealed class ProcessingLogTests
{
    [Fact]
    public void GivenValidTaskId_WhenCreateTaskCompletedLog_ThenLogIsCreatedForTaskCompletion()
    {
        var taskId = Guid.NewGuid();
        var beforeCreate = DateTimeOffset.UtcNow;

        var log = ProcessingLog.CreateTaskCompleted(taskId);

        var afterCreate = DateTimeOffset.UtcNow;
        Assert.NotEqual(Guid.Empty, log.Id);
        Assert.Equal(taskId, log.TaskId);
        Assert.Equal(ProcessingLogEventType.TaskCompleted, log.EventType);
        Assert.False(string.IsNullOrWhiteSpace(log.Message));
        Assert.InRange(log.CreatedAt, beforeCreate, afterCreate);
    }

    [Fact]
    public void GivenEmptyTaskId_WhenCreateTaskCompletedLog_ThenDomainValidationExceptionIsThrown()
    {
        Assert.Throws<DomainValidationException>(() => ProcessingLog.CreateTaskCompleted(Guid.Empty));
    }
}
