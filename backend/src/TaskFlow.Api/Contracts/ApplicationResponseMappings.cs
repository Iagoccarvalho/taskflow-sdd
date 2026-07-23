namespace TaskFlow.Api.Contracts;

public static class ApplicationResponseMappings
{
    public static TaskResponse ToApiResponse(
        this TaskFlow.Application.Tasks.TaskResponse response)
    {
        return new TaskResponse(
            response.Id,
            response.Title,
            response.Description,
            response.Status,
            response.CreatedAt,
            response.UpdatedAt,
            response.CompletedAt);
    }

    public static ProcessingLogResponse ToApiResponse(
        this TaskFlow.Application.ProcessingLogs.ProcessingLogResponse response)
    {
        return new ProcessingLogResponse(
            response.Id,
            response.TaskId,
            response.EventType,
            response.Message,
            response.CreatedAt);
    }

    public static CompleteTaskResponse ToApiResponse(
        this TaskFlow.Application.ProcessingLogs.CompleteTaskResponse response)
    {
        return new CompleteTaskResponse(
            response.Task.ToApiResponse(),
            response.ProcessingLogCreated);
    }
}
