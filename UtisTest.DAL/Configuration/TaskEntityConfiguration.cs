using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UtisTestTask.DAL.Base;
using UtisTestTask.Entities.Tasks;

namespace UtisTestTask.DAL.Configuration;

public class TaskEntityConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.HasKey(_ => _.TaskId);
        builder.Property(_ => _.Title).HasMaxLength(DatabaseSettings.MaxTitleLength);
        builder.Property(_ => _.Description).HasMaxLength(DatabaseSettings.MaxDescriptionLength);
        builder.HasIndex(_ => _.Status);
    }
}