using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.ProcessingLogs;
using TaskFlow.Application.Tasks;

namespace TaskFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateTaskUseCase>();
        services.AddScoped<ListTasksUseCase>();
        services.AddScoped<GetTaskUseCase>();
        services.AddScoped<UpdateTaskUseCase>();
        services.AddScoped<CompleteTaskUseCase>();
        services.AddScoped<ListTaskProcessingLogsUseCase>();
        services.AddScoped<ListProcessingLogsUseCase>();

        return services;
    }
}
