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
        public DbSet<Article> Articles { get; set; }
        public DbSet<CaseStudy> CaseStudies { get; set; }
        public DbSet<Lead> Leads { get; set; }

    }
}