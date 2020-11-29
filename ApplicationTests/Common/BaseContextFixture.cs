using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;

namespace ApplicationTests.Common
{
    public abstract class BaseContextFixture : IDisposable
    {
        protected BaseContextFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ApplicationTestDatabase")
                .Options;
            _context = new ApplicationDbContext(options);
            SeedDataAsync();
        }
        private readonly ApplicationDbContext _context;
        public IApplicationDbContext Context => _context;
        public void Dispose()
        {
            _context.Dispose();
        }
        public abstract void SeedDataAsync();
    }
}
