using System;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Specs.Steps
{
    public class BaseStep
    {
        protected static T CreateDbContext<T>() where T : DbContext
        {
            var dbContextOptions = new DbContextOptionsBuilder<T>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging(true)
                .Options;
            return (T)Activator.CreateInstance(typeof(T), dbContextOptions);
        }
    }
}