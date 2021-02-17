using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace Repository
{
    public class TestContext : DbContext
    {
        public TestContext() { }

        public TestContext(DbContextOptions<TestContext> options) : 
            base(options) { }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Category> Answer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>((e) =>
            {
                e.HasData(
                    new Category { Id = 1, Name = "Comedia" },
                    new Category { Id = 2, Name = "Vida" },
                    new Category { Id = 2, Name = "Vida" }
                );
            });

            modelBuilder.Entity<Question>((e) =>
            {
                e.HasData(
                    new Question { Id = 1, Name = "Entity1" },
                    new Question { Id = 2, Name = "Entity2" }
                );
            });
        }
    }
}
