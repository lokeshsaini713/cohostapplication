using Microsoft.EntityFrameworkCore;
using Shared.Model;
using Shared.Model.Entities;
using System.Collections.Generic;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Shared.Model.UserDetail> UserDetail { get; set; }
        // Add DbSet for other tables

        // Optional: OnModelCreating for custom mapping
    }
}