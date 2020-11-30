using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Subarticle> Subarticles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<AccessRequest> AccessRequests { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is DatedEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));
            foreach (var entityEntry in entries)
            {
                ((DatedEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((DatedEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }


            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Article>().HasIndex(a => a.Title).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(c => c.Title).IsUnique();
            modelBuilder.Entity<Subarticle>();
            modelBuilder.Entity<MediaFile>();
            modelBuilder.Entity<AccessRequest>().HasIndex(ac => new { ac.ArticleId, ac.ProfileId, ac.AccessType }).IsUnique();
            modelBuilder.Entity<Profile>();


            base.OnModelCreating(modelBuilder);
        }
    }
}
