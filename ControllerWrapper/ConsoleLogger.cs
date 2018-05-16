using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerWrapper
{

    public static class ConsoleLogger
    {
        public enum Verbosity { Critical, Error, Warning, Info, Debug }
        public static Verbosity LogLevel = Verbosity.Info;
        private static Dictionary<Verbosity,string> LastMessage = new Dictionary<Verbosity, string>();
        private static void Write(string message, Verbosity verbosity = Verbosity.Debug)
        {
            if (!LastMessage.Keys.Contains(verbosity) || LastMessage[verbosity] != message)
            {
                Console.WriteLine(message);
                LastMessage[verbosity] = message;
            }
        }
        public static void Debug(string message)
        {
            if (LogLevel >= Verbosity.Debug)
            {
                Write(message, Verbosity.Debug);
            }
        }
        public static void Info(string message)
        {
            if (LogLevel >= Verbosity.Info)
            {
                Write(message, Verbosity.Info);
            }
        }
        public static void Warning(string message)
        {
            if (LogLevel >= Verbosity.Warning)
            {
                Write(message, Verbosity.Warning);
            }
        }
        public static void Error(string message)
        {
            if (LogLevel >= Verbosity.Error)
            {
                Write(message, Verbosity.Error);
            }
        }
        public static void Critical(string message)
        {
            if (LogLevel >= Verbosity.Critical)
            {
                Write(message, Verbosity.Critical);
            }
        }

    }
}
