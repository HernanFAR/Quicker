using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Test.Common.Repository
{
    public class TestContext : DbContext
    {
        public TestContext([NotNull] DbContextOptions options) : 
            base(options) { }

        public DbSet<TestModel> TestModels { get; set; }

        public DbSet<TestModelRelation> TestModelRelations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TestModelRelation>(e => {
                e.HasIndex(e => e.UniqueName)
                    .IsUnique();
            });
        }
    }
}
