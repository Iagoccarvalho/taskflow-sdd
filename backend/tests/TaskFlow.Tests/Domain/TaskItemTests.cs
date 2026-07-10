using TaskFlow.Domain.Common;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Tests.Domain;

public sealed class TaskItemTests
{
    [Fact]
    public void GivenValidTitle_WhenCreateTask_ThenTaskKeepsTitleAndGeneratesId()
    {
        var task = TaskItem.Create("Write domain tests");

        Assert.NotEqual(Guid.Empty, task.Id);
        Assert.Equal("Write domain tests", task.Title);
    }

    [Fact]
    public void GivenValidTitle_WhenCreateTask_ThenTaskStartsAsPending()
    {
        var task = TaskItem.Create("Write domain tests");

        Assert.Equal(TaskItemStatus.Pending, task.Status);
        Assert.Null(task.CompletedAt);
    }

    [Fact]
    public void GivenValidTitle_WhenCreateTask_ThenCreatedAtIsFilled()
    {
        var beforeCreate = DateTimeOffset.UtcNow;

        var task = TaskItem.Create("Write domain tests");

        var afterCreate = DateTimeOffset.UtcNow;
        Assert.InRange(task.CreatedAt, beforeCreate, afterCreate);
        Assert.Equal(task.CreatedAt, task.UpdatedAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GivenInvalidTitle_WhenCreateTask_ThenDomainValidationExceptionIsThrown(string? title)
    {
        Assert.Throws<DomainValidationException>(() => TaskItem.Create(title!));
    }

    [Fact]
    public void GivenDescription_WhenCreateTask_ThenDescriptionIsStored()
    {
        var task = TaskItem.Create("Write domain tests", "Cover creation rules");

        Assert.Equal("Cover creation rules", task.Description);
    }

    [Fact]
    public void GivenNoDescription_WhenCreateTask_ThenDescriptionIsOptional()
    {
        var task = TaskItem.Create("Write domain tests");

        Assert.Null(task.Description);
    }

    [Fact]
    public void GivenPendingTask_WhenCompleteTask_ThenCompletionSucceeds()
    {
        var task = TaskItem.Create("Write domain tests");

        var completed = task.Complete();

        Assert.True(completed);
    }

    [Fact]
    public void GivenPendingTask_WhenCompleteTask_ThenStatusChangesToCompleted()
    {
        var task = TaskItem.Create("Write domain tests");

        task.Complete();

        Assert.Equal(TaskItemStatus.Completed, task.Status);
    }

    [Fact]
    public void GivenPendingTask_WhenCompleteTask_ThenCompletedAtIsFilled()
    {
        var task = TaskItem.Create("Write domain tests");
        var beforeComplete = DateTimeOffset.UtcNow;

        task.Complete();

        var afterComplete = DateTimeOffset.UtcNow;
        Assert.NotNull(task.CompletedAt);
        Assert.InRange(task.CompletedAt.Value, beforeComplete, afterComplete);
        Assert.Equal(task.CompletedAt, task.UpdatedAt);
    }

    [Fact]
    public void GivenCompletedTask_WhenCompleteTaskAgain_ThenCompletionIsIdempotent()
    {
        var task = TaskItem.Create("Write domain tests");
        task.Complete();
        var firstCompletedAt = task.CompletedAt;
        var firstUpdatedAt = task.UpdatedAt;

        var completedAgain = task.Complete();

        Assert.False(completedAgain);
        Assert.Equal(TaskItemStatus.Completed, task.Status);
        Assert.Equal(firstCompletedAt, task.CompletedAt);
        Assert.Equal(firstUpdatedAt, task.UpdatedAt);
    }
}
