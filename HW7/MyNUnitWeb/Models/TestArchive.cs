using Microsoft.EntityFrameworkCore;

namespace MyNUnitWeb.Models
{
    public class TestArchive : DbContext
    {
        public DbSet<TestRunModel> TestRuns { get; set; }

        public TestArchive(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=TestArchive;Trusted_Connection=True;");
    }
}