using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.ProcessingLogs;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Infrastructure.Persistence;

public sealed class TaskFlowDbContext : DbContext
{
    public TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    public DbSet<ProcessingLog> ProcessingLogs => Set<ProcessingLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskFlowDbContext).Assembly);
    }
}
