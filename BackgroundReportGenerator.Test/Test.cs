
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BackgroundReportGenerator.Test.Connections;
using BackgroundReportGenerator.Test.Models;

namespace BackgroundReportGenerator.Test
{
    public class Test
    {

        static async Task PreformCSVAsync()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseSqlServer("Server=JENZ;Database=LibraryDB;Encrypt=False;TrustServerCertificate=False;Trusted_Connection=True;")
                .Options;

            var dbContext = new Context(options);
            var process = new TaskProcess(dbContext);


            var tracker = new ReportGenerationTracker()
            {
                IsCompleted = true,
            };
            process.SetOnCompleteCallback(() =>
            {
                process.UpdateTracker((ReportGenerationTracker u) => u.Id == 1, tracker);
            });
            static bool predicate(User u) => u.Id != 0;
            await process.PerformDataReadToCSV<User>(predicate, 10, "C://LibraryReport/file.csv");
        }


        [Fact]
        static async void StartBackgroundTask()
        {
            var backgroundTask = new TaskManager();
            backgroundTask.StartAsync(async (cancellationToken) =>
            {
                await PreformCSVAsync();
            });
        }
    }
}
