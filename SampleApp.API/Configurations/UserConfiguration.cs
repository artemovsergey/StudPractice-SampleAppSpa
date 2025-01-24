using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleApp.API.Entities;

namespace SampleApp.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{

    public void Configure(EntityTypeBuilder<User> builder){
        builder.Property(e => e.Login)
        .IsRequired()
        .HasMaxLength(50);
    }
    
}