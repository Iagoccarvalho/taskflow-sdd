using TaskFlow.Application.Common;

namespace TaskFlow.Application.Tasks;

internal static class TaskRequestValidator
{
    public static ApplicationError? ValidateTitle(string? title)
    {
        return string.IsNullOrWhiteSpace(title)
            ? new ApplicationError("task.title.required", "Task title is required.")
            : null;
    }
}
