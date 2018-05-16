using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ControllerWrapper
{
    public class Worker : IDisposable
    {
        private readonly Timer _timer;
        private int PeriodInMs;
        private Action Work;

        public Worker(Action work, int milliseconds = 1000 / 60)
        {
            Work = work;
            PeriodInMs = milliseconds;
            _timer = new Timer(_ => OnTick(), null, Timeout.Infinite, Timeout.Infinite);
        }

        public void Start()
        {
            _timer.Change(0, PeriodInMs);
        }

        public void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void OnTick()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            try
            {
                Work();
            }
            finally
            {
                _timer.Change(PeriodInMs, PeriodInMs);
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
