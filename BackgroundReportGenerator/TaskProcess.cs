using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Globalization;


namespace BackgroundReportGenerator
{
    public class TaskProcess
    {
        private DataAccessor accessor;
        private Action onCompleteCallback;

        public TaskProcess(DbContext _context)
        {
            this.accessor = DataAccessor.GetInstance(_context);
        }

        public async Task PerformDataReadToCSV<T>(Func<T, bool> predicate, uint limit, string path) where T : class
        {
            string cursor = null;

            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(new List<T>());

                do
                {
                    List<T> records = accessor.Fetch(predicate, ref cursor, limit);
                    if (records.Count > 0)
                    {
                        csv.WriteRecords(records);
                    }
                } while (cursor != null);
                CallOnCompleteCallback();
            }
        }

        public void UpdateTracker<T>(Func<T, bool> predicate, T data) where T : class
        {
            this.accessor.Update(predicate, data);
        }

        public void SetOnCompleteCallback(Action callback)
        {
            this.onCompleteCallback = callback;
        }

        private void CallOnCompleteCallback()
        {
            onCompleteCallback.Invoke();
        }
    }
}
