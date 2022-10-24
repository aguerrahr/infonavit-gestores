using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GestoresAPI.Models.Contexts
{
    public class GestoresAPIContext : DbContext
    {
        public GestoresAPIContext(DbContextOptions<GestoresAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Modules> Modules { get; set; }
        public DbSet<Faculties> Faculties { get; set; }
        public DbSet<Settings> Settings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseLazyLoadingProxies();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
              .Entity<Employee>()
                .HasOne<Job>(e => e.Job)
                    .WithMany(j => j.Employees)
                .HasForeignKey(e => e.IdJob);
            modelBuilder
                .Entity<Role>()
                .HasMany(r => r.Employees)
                .WithMany(e => e.Roles)
                .UsingEntity<Dictionary<string, object>>("RolesEmployee",
                    j => j
                        .HasOne<Employee>()
                        .WithMany()
                        .HasForeignKey("EmployeeId"),
                    j => j
                        .HasOne<Role>()
                        .WithMany()
                        .HasForeignKey("RoleId"));
            modelBuilder
                .Entity<Job>()
                .HasMany(j => j.JobManagers)
                .WithMany(j => j.SubJobs)
                .UsingEntity<Dictionary<string, object>>("JobHierarchy",
                    j => j
                        .HasOne<Job>()
                        .WithMany()
                        .HasForeignKey("JobId"),
                    j => j
                        .HasOne<Job>()
                        .WithMany()
                        .HasForeignKey("JobIdChild"));
            modelBuilder
                .Entity<Employee>()
                .HasMany(j => j.Managers)
                .WithMany(j => j.SubEmployees)
                .UsingEntity<Dictionary<string, object>>("Managers",
                    j => j
                        .HasOne<Employee>()
                        .WithMany()
                        .HasForeignKey("ManagerId"),
                    j => j
                        .HasOne<Employee>()
                        .WithMany()
                        .HasForeignKey("EmployeeId"));
            modelBuilder
                .Entity<Job>()
                .HasMany(r => r.Modules)
                .WithMany(r => r.Jobs)
                .UsingEntity<Dictionary<string, object>>("JobsModules",
                    r => r
                        .HasOne<Modules>()
                        .WithMany()
                        .HasForeignKey("ModuleId"),
                    r => r
                        .HasOne<Job>()
                        .WithMany()
                        .HasForeignKey("JobId"));
            modelBuilder
                .Entity<Job>()
                .HasMany(r => r.Faculties)
                .WithMany(r => r.Jobs)
                .UsingEntity<Dictionary<string, object>>("JobsFaculties",
                    r => r
                        .HasOne<Faculties>()
                        .WithMany()
                        .HasForeignKey("FacultyId"),
                    r => r
                        .HasOne<Job>()
                        .WithMany()
                        .HasForeignKey("JobId"));
            modelBuilder
                .Entity<Modules>(entity =>
                {
                    entity.Property(p => p.Icon).IsRequired(required: false);
                    entity.Property(p => p.RegisteredAt).IsRequired(required: false);
                });
            modelBuilder
               .Entity<Faculties>(entity =>
               {
                   entity.Property(p => p.Icon).IsRequired(required: false);
                   entity.Property(p => p.RegisteredAt).IsRequired(required: false);
               });
        }
    }
}
