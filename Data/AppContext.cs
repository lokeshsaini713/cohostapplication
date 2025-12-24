using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        // Add DbSet for other tables

        // Optional: OnModelCreating for custom mapping
    }
}