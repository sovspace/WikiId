using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Article> Articles { get; set; }
        DbSet<Subarticle> Subarticles { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Profile> Profiles { get; set; }
        DbSet<MediaFile> MediaFiles { get; set; }
        DbSet<AccessRequest> AccessRequests { get; set; }
        public Task<int> SaveChangesAsync();
    }
}
