using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options) { }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Business> Business { get; set; }
        public DbSet<Customers> Customer { get; set; }
        public DbSet<CompanyDocuments> CompanyDocuments { get; set; }
        public DbSet<ResponsibleUsers> ResponsibleUsers { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<ProjectTasks> ProjectTasks { get; set; }
        public DbSet<ProjectDocuments> ProjectDocuments { get; set; }
        public DbSet<ProjectComments> ProjectComments { get; set; }
        public DbSet<Scheduler> Scheduler { get; set; }
        public DbSet<SchedulerPriority> SchedulerPriority { get; set; }
        public DbSet <TaskPlayers> TaskPlayers { get; set; }
        public DbSet <Notifications>  Notifications { get; set; }
        public DbSet<Meetings> Meetings { get; set; }
        public DbSet<ResponsibleMeetings> ResponsibleMeetings { get; set; }
    }
}
