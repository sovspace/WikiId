using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;

namespace ApplicationTests.Common
{
    public abstract class BaseContextFixture : IDisposable
    {
        private static int _fixtureNo = 0;
        private static object _lock = new object();
        protected BaseContextFixture()
        {
            lock (_lock)
            {
                _fixtureNo++;
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: $"ApplicationTestDatabaseFixtureNo{_fixtureNo}")
                    .Options;
                _context = new ApplicationDbContext(options);
            }
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
