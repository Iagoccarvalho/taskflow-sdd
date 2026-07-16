using TaskFlow.Domain.Tasks;

namespace TaskFlow.Application.Tasks;

public static class TaskMappings
{
    public static TaskResponse ToResponse(this TaskItem task)
    {
        return new TaskResponse(
            task.Id,
            task.Title,
            task.Description,
            task.Status.ToString(),
            task.CreatedAt,
            task.UpdatedAt,
            task.CompletedAt);
    }
}
