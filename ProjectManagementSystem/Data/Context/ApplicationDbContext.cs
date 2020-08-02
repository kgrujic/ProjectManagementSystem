using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Models.ProjectViewModels;
using Task = ProjectManagementSystem.Models.Task;

namespace ProjectManagementSystem.ProjectManagementSystemDatabase.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.UserName)
                .IsUnique();
            
               
            builder.Entity<Project>()
                .HasOne(p => p.ProjectManager)
                .WithMany(b => b.Projects)
                .HasForeignKey(p => p.ProjectManagerId); 
            
            builder.Entity<Task>()
                .HasOne(p => p.Assignee)
                .WithMany(b => b.Tasks)
                .HasForeignKey(p => p.AssigneeId); 
            
            builder.Entity<Task>()
                .HasOne(p => p.Project)
                .WithMany(b => b.Tasks)
                .HasForeignKey(p => p.ProjectCode);
            
        }
    }
}
