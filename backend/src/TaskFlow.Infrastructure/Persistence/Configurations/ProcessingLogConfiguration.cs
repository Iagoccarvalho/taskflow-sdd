using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.ProcessingLogs;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Infrastructure.Persistence.Configurations;

public sealed class ProcessingLogConfiguration : IEntityTypeConfiguration<ProcessingLog>
{
    public void Configure(EntityTypeBuilder<ProcessingLog> builder)
    {
        builder.ToTable("task_processing_logs");

        builder.HasKey(log => log.Id);

        builder.Property(log => log.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(log => log.TaskId)
            .HasColumnName("task_item_id")
            .IsRequired();

        builder.Property(log => log.EventType)
            .HasColumnName("event_type")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(log => log.Message)
            .HasColumnName("message")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(log => log.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasOne<TaskItem>()
            .WithMany()
            .HasForeignKey(log => log.TaskId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(log => new { log.TaskId, log.EventType })
            .IsUnique()
            .HasDatabaseName("ux_task_processing_logs_task_item_id_event_type");
    }
}
