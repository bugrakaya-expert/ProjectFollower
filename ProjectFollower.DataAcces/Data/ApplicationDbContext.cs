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
    }
}
