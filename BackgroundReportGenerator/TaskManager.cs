
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundReportGenerator
{
    public class TaskManager
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        public TaskManager()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task StartAsync(Func<CancellationToken, Task> callback)
        {
            if (_cancellationTokenSource.Token.IsCancellationRequested)
            {
                return;
            }

            try
            {
                await callback(_cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                throw new Exception("Break");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
