﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
            Console.WriteLine("-lstickthrow [number]\tSets the maximum throw of the left stick from 0-1. Default is 1.");
            Console.WriteLine("-rstickthrow [number]\tSets the maximum throw of the right stick from 0-1. Default is 1.");
            Console.WriteLine("-toggletriggers \tLeft and Right trigger are toggled each time they are pressed");
            Console.WriteLine("-eternalholds \tHeld inputs last until the next held input");
            Console.WriteLine("-minheldframes [frames]\tSets the minimum duration an input will be held.");
            Console.WriteLine("-maxsleepframes [frames]\tSets the maximum duration between inputs.");
            Console.WriteLine("-maxheldframes [frames]\tSets the maximum duration a normal input will be held.");
            Console.WriteLine("-maxholdframes [frames]\tSets the maximum duration a hold input will be held.");
            Console.WriteLine("-forcefocus [program]\tMakes sure the given program has focus before sending each input.");
            Console.WriteLine("-forcesavebackup [seconds]\tTells the core to back up the save file every given interval.");
            Console.WriteLine("-savebackupendpoint [url]\tSets the URL that tells the core to back up the save file.");

        }

        static void Main(string[] args)
        {

            string newInputEndpoint = "http://127.0.0.1:5000/gbmode_input_request_bizhawk";
            string doneInputEndpoint = "http://127.0.0.1:5000/gbmode_input_complete";
            int controller = 1;
            int minHeldFrames = 1;
            int maxSleepFrames = 100;
            int maxHeldFrames = 100;
            int maxHoldFrames = 24;
            bool toggleTriggers = false;
            bool eternalHolds = false;
            string forceFocusProgram = null;
            int forceSaveBackupSeconds = 0;
            string saveBackupEndpoint = "http://127.0.0.1:5000/back_up_savestate";

#if DEBUG
            ConsoleLogger.LogLevel = ConsoleLogger.Verbosity.Debug;
#endif

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
                        case "-v":
                            ConsoleLogger.LogLevel = ConsoleLogger.Verbosity.Debug;
                            ConsoleLogger.Debug("Verbose output.");
                            break; ;
                        case "-inputendpoint":
                            newInputEndpoint = args[i + 1];
                            ConsoleLogger.Info($"Input endpoint: {newInputEndpoint}");
                            break;
                        case "-doneendpoint":
                            doneInputEndpoint = args[i + 1];
                            ConsoleLogger.Info($"Done endpoint: {doneInputEndpoint}");
                            break;
                        case "-savebackupendpoint":
                            saveBackupEndpoint = args[i + 1];
                            ConsoleLogger.Info($"Save Backup endpoint: {saveBackupEndpoint}");
                            break;
                        case "-controller":
                            controller = int.Parse(args[i + 1]);
                            ConsoleLogger.Info($"Simulating controller #{controller}");
                            break;
                        case "-lstickthrow":
                            TPPInput.LThrowGlobalMult = double.Parse(args[i + 1]);
                            ConsoleLogger.Info($"Left Stick max throw set to {(TPPInput.LThrowGlobalMult * 100).ToString("0")}%");
                            break;
                        case "-rstickthrow":
                            TPPInput.RThrowGlobalMult = double.Parse(args[i + 1]);
                            ConsoleLogger.Info($"Right Stick max throw set to {(TPPInput.RThrowGlobalMult * 100).ToString("0")}%");
                            break;
                        case "-toggletriggers":
                            toggleTriggers = true;
                            ConsoleLogger.Info("Left and Right Triggers will be toggled");
                            break;
                        case "-eternalholds":
                            eternalHolds = true;
                            maxHoldFrames = 0;
                            ConsoleLogger.Info("Held inputs will last until the next held input");
                            break;
                        case "-minheldframes":
                            minHeldFrames = int.Parse(args[i + 1]);
                            ConsoleLogger.Info($"Minimum Held Frames: {minHeldFrames}");
                            break;
                        case "-maxsleepframes":
                            maxSleepFrames = int.Parse(args[i + 1]);
                            ConsoleLogger.Info($"Maximum Sleep Frames: {maxSleepFrames}");
                            break;
                        case "-maxheldframes":
                            maxHeldFrames = int.Parse(args[i + 1]);
                            ConsoleLogger.Info($"Maximum Held Frames: {maxHeldFrames}");
                            break;
                        case "-maxholdframes":
                            maxHoldFrames = int.Parse(args[i + 1]);
                            ConsoleLogger.Info($"Maximum Hold Input Frames: {maxHoldFrames}");
                            break;
                        case "-forcefocus":
                            forceFocusProgram = args[i + 1];
                            ConsoleLogger.Info($"Forcing {forceFocusProgram} to have focus for each input");
                            break;
                        case "-forcesavebackup":
                            forceSaveBackupSeconds = int.Parse(args[i + 1]);
                            ConsoleLogger.Info($"Forcing save backup every {forceSaveBackupSeconds} seconds");
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

            var inputFetcher = new FetchInput(scpBus, controller, minHeldFrames, maxHeldFrames, maxSleepFrames, maxHoldFrames, toggleTriggers, eternalHolds, newInputEndpoint, doneInputEndpoint, forceFocusProgram);

            var worker = new Worker(() => inputFetcher.Frame());
            //var stopwatch = new Stopwatch();
            //var worker = new Worker(() => ConsoleLogger.Info($"{stopwatch.ElapsedTicks}"));
            //stopwatch.Start();

            worker.Start();

            var willBackupSave = forceSaveBackupSeconds > 0 && !string.IsNullOrWhiteSpace(saveBackupEndpoint);

            while (true)
            {
                for (var i = 0; i < forceSaveBackupSeconds; i += forceSaveBackupSeconds / 10)
                {
                    if (willBackupSave)
                        ConsoleLogger.Info($"{forceSaveBackupSeconds - i} seconds until save backup...");
                    Thread.Sleep(forceSaveBackupSeconds * 100);
                }
                if (willBackupSave)
                {
                    using (var webClient = new ImpatientWebClient(forceSaveBackupSeconds * 1000))
                    {
                        ConsoleLogger.Info("Backing up save...");
                        webClient.DownloadStringAsync(new Uri(saveBackupEndpoint));
                    }
                }
            }
        }
    }
}
