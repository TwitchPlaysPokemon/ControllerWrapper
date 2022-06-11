using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ControllerWrapper
{
    public class Worker
    {
        private readonly FPSTimer _timer;
        private Action Work;

        public Worker(Action work, int fps = 60)
        {
            Work = work;
            _timer = new FPSTimer(OnTick, fps);
        }

        public void Start() => _timer.Start();

        public void Stop() => _timer.Stop();

        private void OnTick()
        {
            try
            {
                Work();
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error($"{ex.Message}: {ex.StackTrace}");
            }
        }
    }
}
