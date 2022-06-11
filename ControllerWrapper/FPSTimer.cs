using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControllerWrapper
{
    public class FPSTimer
    {
        private int _fps = 60;
        public Action OnTick { get; set; }
        private int CurrentRun = 0;

        public FPSTimer(Action onTick, int fps = 60)
        {
            OnTick = onTick;
            _fps = fps;
        }

        private async void Run()
        {
            await Task.Run(() =>
            {
                var runId = CurrentRun;
                var interval = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / _fps);
                var nextTick = DateTime.Now + interval;
                var stopwatch = new Stopwatch();
                ConsoleLogger.Debug($"Starting FPS Timer {runId}");
                stopwatch.Start();
                while (runId == CurrentRun)
                {
                    while (DateTime.Now < nextTick)
                    {
                        var sleepDuration = nextTick - DateTime.Now;
                        if (sleepDuration.TotalMilliseconds < 1)
                            sleepDuration = TimeSpan.FromMilliseconds(1);
                        ConsoleLogger.Debug($"Sleeping for {sleepDuration.TotalMilliseconds}ms");
                        Thread.Sleep(sleepDuration);
                    }
                    nextTick += interval;
                    if (runId == CurrentRun)
                    {
                        ConsoleLogger.Debug($"FPS Timer {runId} ticked at {stopwatch.Elapsed} since starting");
                        OnTick?.Invoke();
                    }
                }
                ConsoleLogger.Debug($"Stopping FPS Timer {runId}");
                stopwatch.Stop();
            });
        }

        public void Start() => Run();
        public void Stop() => CurrentRun = (CurrentRun + 1) % int.MaxValue;
    }
}
