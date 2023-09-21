
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

            var db = new Context(options);
            TaskManager task = new TaskManager(db);

            static bool predicate1(User u) => u.Id == 1;
            var data = new User()
            {
                Username = "Kaycee",
            };

            task.SetOnCompleteCallback(() => task.UpdateTracker(predicate1, data));

            static bool predicate(User u) => u.Id != 0;
            await task.PerformDataReadToCSV<User>(predicate, 10, "C://LibraryReport/file.csv");
        }


        [Fact]
        static async void StartBackgroundTask()
        {
            TaskProcess backgroundTask = new();

            await backgroundTask.StartAsync(e => PreformCSVAsync());
        }
    }
}
