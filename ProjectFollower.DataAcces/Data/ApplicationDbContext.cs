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
        public DbSet<CompanyType> CompanyType { get; set; }
        public DbSet<CompanyDocuments> CompanyDocuments { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<ProjectTasks> ProjectTasks { get; set; }
        public DbSet<ResponsibleUsers> ResponsibleUsers { get; set; }
    }
}
