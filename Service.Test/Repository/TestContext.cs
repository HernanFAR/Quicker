using Microsoft.EntityFrameworkCore;
using Service.Test.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Test.Repository
{
    public class TestContext : DbContext
    {
        public TestContext() { }

        public TestContext(DbContextOptions<TestContext> options) : 
            base(options) { }

        public DbSet<FirstLevelEntity> FirstLevelEntities { get; set; }

        public DbSet<SecondLevelEntity> SecondLevelEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FirstLevelEntity>((e) =>
            {
                e.HasData(
                    new FirstLevelEntity { Id = 1, Name = "Entity1" },
                    new FirstLevelEntity { Id = 2, Name = "Entity2" }
                );
            });

            modelBuilder.Entity<SecondLevelEntity>((e) =>
            {
                e.HasData(
                    new SecondLevelEntity { Id = 1, Name = "Entity1", FirstLevelEntityId = 1 },
                    new SecondLevelEntity { Id = 2, Name = "Entity2", FirstLevelEntityId = 1 }
                );
            });
        }
    }
}
