using Microsoft.EntityFrameworkCore;

namespace MyNUnitWeb.Models
{
    public class TestArchive : DbContext
    {

        public TestArchive(DbContextOptions<TestArchive> options)
                : base(options)
        {
        }

        public DbSet<TestRunModel> RunHistory { get; set; }

        public DbSet<AssemblyReportModel> ReportAssemblies { get; set; }

        public DbSet<TestReportModel> ReportsTests { get; set; }
    }
}
