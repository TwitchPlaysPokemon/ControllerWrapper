using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScpDriverInterface;

namespace ControllerWrapper
{
    class Program
    {

        static void PrintHelpText()
        {
            Console.WriteLine("Options:");
            Console.WriteLine("-help\t\tPrints this text");
            Console.WriteLine("-inputendpoint [url]\tSets the URL to fetch inputs from");
            Console.WriteLine("-doneendpoint [url]\tSets the URL that marks inputs complete");
            Console.WriteLine("-controller [number]\tChooses the controller number to simulate. Default is 1.");
            Console.WriteLine("-minheldframes [frames]\tSets the minimum duration an input will be held.");
            Console.WriteLine("-maxsleepframes [frames]\tSets the maximum duration between inputs.");

        }

        static void Main(string[] args)
        {

            string newInputEndpoint = "http://127.0.0.1:5000/gbmode_input_request_bizhawk";
            string doneInputEndpoint = "http://127.0.0.1:5000/gbmode_input_complete";
            int controller = 1;
            int minHeldFrames = 1;
            int maxSleepFrames = 100;

            try
            {
                for (var i = 0; i < args.Length; i++)
                {
                    switch (args[i].ToLower())
                    {
                        case "-h":
                        case "-help":
                            PrintHelpText();
                            return;
                        case "-inputendpoint":
                            newInputEndpoint = args[i + 1];
                            ConsoleLogger.Info($"Input endpoint: {newInputEndpoint}");
                            break;
                        case "-doneendpoint":
                            doneInputEndpoint = args[i + 1];
                            ConsoleLogger.Info($"Done endpoint: {doneInputEndpoint}");
                            break;
                        case "-controller":
                            controller = int.Parse(args[i + 1]);
                            ConsoleLogger.Info($"Simulating controller #{controller}");
                            break;
                        case "-minheldframes":
                            minHeldFrames = int.Parse(args[i + 1]);
                            ConsoleLogger.Info($"Minimum Held Frames: {minHeldFrames}");
                            break;
                        case "-maxsleepframes":
                            maxSleepFrames = int.Parse(args[i + 1]);
                            ConsoleLogger.Info($"Maximum Sleep Frames: {maxSleepFrames}");
                            break;
                    }
                }
            }
            catch(Exception e)
            {
                ConsoleLogger.Critical(e.Message);
                PrintHelpText();
                return;
            }

            ScpBus scpBus;
            try
            {
                scpBus = new ScpBus();
            }
            catch
            {
                ConsoleLogger.Critical("Could not create virtual controller. Please make sure the SCP Virtual Bus Driver is installed.");
                return;
            }

            scpBus.PlugIn(1);
            ConsoleLogger.Info("Virtual controller connected.");


            Console.CancelKeyPress += delegate {
                scpBus.UnplugAll();
                ConsoleLogger.Info("Virtual controller(s) disconnected.");
                scpBus.Dispose();
            };

            var inputFetcher = new FetchInput(scpBus, controller, minHeldFrames, maxSleepFrames, newInputEndpoint, doneInputEndpoint);

            var worker = new Worker(() => inputFetcher.Frame());

            worker.Start();

            while (true)
            {

            }
        }
    }
}
