﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScpDriverInterface;
using Newtonsoft.Json;

namespace ControllerWrapper
{
    class FetchInput
    {
        private TPPInput currentInput { get; set; }
        private Queue<TPPInput> currentSeries { get; set; }
        private ImpatientWebClient webClient { get; set; }
        private string newInputEndpoint { get; set; }
        private string doneInputEndpoint { get; set; }
        private ScpBus scpBus;
        private int scpController;
        private int maxSleep;
        private int minHeld;


        public FetchInput(ScpBus ctrlBus, int controller, int minHeldFrames, int maxSleepFrames, string newInputUrl, string doneInputUrl)
        {
            scpBus = ctrlBus;
            scpController = controller;
            maxSleep = maxSleepFrames;
            minHeld = minHeldFrames;
            newInputEndpoint = newInputUrl;
            doneInputEndpoint = doneInputUrl;
            currentInput = new TPPInput();
            currentSeries = new Queue<TPPInput>();
            webClient = new ImpatientWebClient();
        }

        public void Frame()
        {
            if (!currentInput.active)
            {
                if (currentSeries.Any())
                {
                    currentInput = currentSeries.Dequeue();
                    currentInput.active = true;
                }
                else
                {
                    try
                    {
                        var response = webClient.DownloadString($"{newInputEndpoint}?client=xbox");
                        ConsoleLogger.Debug(response);
                        var nextInput = JsonConvert.DeserializeObject<TPPInput>(response);
                        if (!(currentInput.Hold && nextInput.IsEmpty))
                        {
                            currentInput = nextInput;
                        }
                    }
                    catch (Exception e)
                    {
                        ConsoleLogger.Warning($"Error communicating with core: {e.Message}");
                    }
                    if (currentInput.Series != null)
                    {
                        currentSeries = new Queue<TPPInput>(currentInput.Series);
                        Frame();
                        return;
                    }
                    else if (!string.IsNullOrWhiteSpace(currentInput.Macro))
                    {
                        var inputs = JsonConvert.DeserializeObject <List<TPPInput>>(JsonConvert.SerializeObject((MacroBank.Macros.FirstOrDefault(m => m.Name.ToLower() == currentInput.Macro.ToLower()) ?? new MacroBank.Macro() { Inputs = new List<TPPInput>() }).Inputs));
                        foreach (var input in inputs)
                        {
                            input.Held_Frames = input.Held_Frames == 0 ? Math.Max(4, currentInput.Held_Frames / inputs.Count()) : input.Held_Frames;
                            input.Sleep_Frames = input.Sleep_Frames == 0 ? Math.Max(4, currentInput.Sleep_Frames / inputs.Count()) : input.Sleep_Frames;
                        }
                        currentSeries = new Queue<TPPInput>(inputs);
                        Frame();
                        return;
                    }
                    else
                    {
                        currentInput.active = true;
                        var frameLength = currentInput.Held_Frames + currentInput.Sleep_Frames;
                        currentInput.Held_Frames = Math.Min(Math.Max(currentInput.Held_Frames, minHeld), frameLength - 1);
                        currentInput.Sleep_Frames = Math.Min(Math.Min(currentInput.Sleep_Frames, maxSleep), frameLength - currentInput.Held_Frames);
                    }
                }
            }
            if (currentInput.active)
            {
                if (currentInput.Held_Frames-- <= 0)
                {
                    if (currentInput.Sleep_Frames-- <= 0)
                    {
                        try
                        {
                            webClient.DownloadString($"{doneInputEndpoint}?client=xbox");
                            currentInput.active = false;
                        }
                        catch (Exception e)
                        {
                            ConsoleLogger.Warning($"Error communicating with core: {e.Message}");
                        }
                    }
                }
            }
            if (currentInput.active && currentInput.Held_Frames > 0)
            {
                var input = currentInput.ToX360();
                scpBus.Report(scpController, input.GetReport());
                ConsoleLogger.Info($"Buttons: {input.Buttons}");
            }
            else if (!currentInput.Hold)
            {
                var input = new TPPInput().ToX360();
                scpBus.Report(scpController, input.GetReport());
                ConsoleLogger.Info($"Buttons: {input.Buttons}");
            }
        }
    }
}