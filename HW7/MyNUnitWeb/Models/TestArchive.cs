using Microsoft.EntityFrameworkCore;

namespace MyNUnitWeb.Models
{
    public class TestArchive : DbContext
    {
        /// <summary>
        /// Constructir
        /// </summary>
        /// <param name="options">Context options</param>
        public TestArchive(DbContextOptions<TestArchive> options)
                : base(options)
        {
        }

        /// <summary>
        /// Collections of runs from db
        /// </summary>
        public DbSet<TestRunModel> RunModels { get; set; }

        /// <summary>
        /// Conjugated data
        /// </summary>
        public DbSet<AssemblyReportModel> AssemblyReportModels { get; set; }

        /// <summary>
        /// Conjugated data
        /// </summary>
        public DbSet<TestReportModel> TestReportModels { get; set; }
    }
}
