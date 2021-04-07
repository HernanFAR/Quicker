using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Test.Common.Repository
{
    public class TestContext : DbContext
    {
        public TestContext([NotNull] DbContextOptions options) : 
            base(options) { }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<TestModel> TestModels { get; set; }

        public DbSet<TestModelRelation> TestModelRelations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TestModelRelation>(e => {
                e.HasIndex(e => e.UniqueName)
                    .IsUnique();
            });

            modelBuilder.Entity<Category>(e => 
            {
                e.HasIndex(e => e.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Product>(e => 
            {
                e.HasIndex(e => e.Name)
                    .IsUnique();

                e.HasIndex(e => e.Code)
                    .IsUnique();
            });
        }
    }
}
