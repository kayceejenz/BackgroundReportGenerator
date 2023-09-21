using BackgroundReportGenerator.Test.Models;
using Microsoft.EntityFrameworkCore;


namespace BackgroundReportGenerator.Test.Connections
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
