using System;
using EnsekMeterReadingCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnsekMeterReadingInfra;

public class EnsekDbContext : DbContext
{
    public DbSet<AccountEntity> Accounts { get; set; }

    public DbSet<MeterReadingEntity> MeterReadings { get; set; }

    public EnsekDbContext(DbContextOptions<EnsekDbContext> options)
       : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AccountEntity>().ToTable("Accounts");
        modelBuilder.Entity<AccountEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<AccountEntity>().Property(x => x.FirstName).IsRequired();
        modelBuilder.Entity<AccountEntity>().Property(x => x.LastName).IsRequired();

        modelBuilder.Entity<MeterReadingEntity>().ToTable("MeterReadings");
        modelBuilder.Entity<MeterReadingEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<MeterReadingEntity>().Property(x => x.AccountId).IsRequired();
        modelBuilder.Entity<MeterReadingEntity>().Property(x => x.ReadingTime).IsRequired();
        modelBuilder.Entity<MeterReadingEntity>().Property(x => x.ReadingValue).IsRequired();

        modelBuilder.Entity<MeterReadingEntity>()
            .HasOne<AccountEntity>()                          
            .WithMany()                               
            .HasForeignKey(mr => mr.AccountId)
            .HasPrincipalKey(a => a.Id)         
            .OnDelete(DeleteBehavior.Cascade);        
    }
}
