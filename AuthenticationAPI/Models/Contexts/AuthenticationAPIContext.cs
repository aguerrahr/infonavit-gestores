using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AuthenticationAPI.Models.Contexts
{
    public class AuthenticationAPIContext : DbContext
    {
        public AuthenticationAPIContext(DbContextOptions<AuthenticationAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Authentication> Authentications { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Job> Jobs { get; set; }

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
              .Entity<Employee>()
                .HasOne(e => e.Authentication)
                .WithOne(a => a.Employee)
                 .HasForeignKey<Authentication>(a => a.ID);
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
                .Entity<Role>()
                .HasMany(r => r.Resources)
                .WithMany(r => r.Roles)
                .UsingEntity<Dictionary<string, object>>("RolesResources",
                    r => r
                        .HasOne<Resource>()
                        .WithMany()
                        .HasForeignKey("ResourceId"),
                    r => r
                        .HasOne<Role>()
                        .WithMany()
                        .HasForeignKey("RoleId"));
        }
        }
}
