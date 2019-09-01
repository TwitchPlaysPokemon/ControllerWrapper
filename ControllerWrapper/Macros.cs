using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerWrapper
{
    class MacroBank
    {
        public class Macro
        {
            public string Name { get; set; }
            public List<TPPInput> Inputs { get; set; }
        }

        public static List<Macro> Macros = new List<Macro>() {
            new Macro() { Name = "psp-dash", Inputs = new List<TPPInput>() { new TPPInput() { Up= true, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput(){ Triangle = true, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Up = true, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Triangle = true, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Up = true, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Triangle = true, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Up = true, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Triangle = true, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Up = true, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Triangle = true, Held_Frames = 4, Sleep_Frames = 4 } } },
            new Macro() { Name = "psp-turn", Inputs = new List<TPPInput>() { new TPPInput() { Left = true, Circle = true, Held_Frames = 4, Sleep_Frames = 4}, new TPPInput() { Left = true, Circle = true, Held_Frames = 4, Sleep_Frames = 4 } }},
            new Macro() { Name = "ps2-done", Inputs = new List<TPPInput>() { new TPPInput() { L1 = true, L2 = true, R1 = true, R2 = true, Triangle = true, Cross = true, Circle = true, Square = true, Held_Frames = 4, Sleep_Frames = 4}} },
            new Macro() { Name = "ps2-dash", Inputs = new List<TPPInput>() { new TPPInput() { Lup = 1, Rdown = 1, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput(){ Rup = 1, Ldown = 1, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Lup = 1, Rdown = 1, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Rup = 1, Ldown = 1, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Lup = 1, Rdown = 1, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Rup = 1, Ldown = 1, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Lup = 1, Rdown = 1, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Rup = 1, Ldown = 1, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Lup = 1, Rdown = 1, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Rup = 1, Ldown = 1, Held_Frames = 4, Sleep_Frames = 4 } } },
            new Macro() { Name = "ps2-turn", Inputs = new List<TPPInput>() { new TPPInput() { L3 = true, R3 = true, Held_Frames = 4, Sleep_Frames = 4}, new TPPInput() { L3 = true, R3 = true, Held_Frames = 4, Sleep_Frames = 4 } }},
            new Macro() { Name = "x360-dash", Inputs = new List<TPPInput>() { new TPPInput() { Lup = 1, Rdown = 1, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput(){ Rup = 1, Ldown = 1, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Lup = 1, Rdown = 1, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Rup = 1, Ldown = 1, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Lup = 1, Rdown = 1, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Rup = 1, Ldown = 1, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Lup = 1, Rdown = 1, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Rup = 1, Ldown = 1, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Lup = 1, Rdown = 1, Held_Frames = 4, Sleep_Frames = 4 }, new TPPInput() { Rup = 1, Ldown = 1, Held_Frames = 4, Sleep_Frames = 4 } } },
            new Macro() { Name = "x360-turn", Inputs = new List<TPPInput>() { new TPPInput() { L3 = true, R3 = true, Held_Frames = 4, Sleep_Frames = 4}, new TPPInput() { L3 = true, R3 = true, Held_Frames = 4, Sleep_Frames = 4 } }}
        };

    }
}
