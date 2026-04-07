using Microsoft.EntityFrameworkCore;
using final_project_backend.Models;

namespace final_project_backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Client>(e =>
        {
            e.HasKey(c => c.Id);
            e.HasIndex(c => c.Email).IsUnique();
        });

        modelBuilder.Entity<Service>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Price).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Appointment>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.PriceAtBooking).HasColumnType("decimal(18,2)");
            e.Property(a => a.Status).HasConversion<int>();

            e.HasOne(a => a.Client)
             .WithMany(c => c.Appointments)
             .HasForeignKey(a => a.ClientId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(a => a.Service)
             .WithMany(s => s.Appointments)
             .HasForeignKey(a => a.ServiceId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(a => a.AssignedStaff)
             .WithMany(u => u.AssignedAppointments)
             .HasForeignKey(a => a.AssignedStaffId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ActivityLog>(e =>
        {
            e.HasKey(l => l.Id);

            e.HasOne(l => l.PerformedByUser)
             .WithMany(u => u.ActivityLogs)
             .HasForeignKey(l => l.PerformedByUserId)
             .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
