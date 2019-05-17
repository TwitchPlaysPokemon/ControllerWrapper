using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScpDriverInterface;

namespace ControllerWrapper
{
    class TPPInput
    {
        //D-Pad
        public bool Up { get; set; }
        public bool Down { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }

        //Face buttons
        public bool A { get; set; }
        public bool B { get; set; }
        public bool X { get; set; }
        public bool Y { get; set; }

        //PlayStation face buttons
        public bool Cross { get; set; }
        public bool Circle { get; set; }
        public bool Square { get; set; }
        public bool Triangle { get; set; }

        public bool Select { get; set; }
        public bool Start { get; set; }

        //Analog sticks
        public bool Lup { get; set; }
        public bool Ldown { get; set; }
        public bool Lleft { get; set; }
        public bool Lright { get; set; }
        public bool L3 { get; set; }
        public bool Rup { get; set; }
        public bool Rdown { get; set; }
        public bool Rleft { get; set; }
        public bool Rright { get; set; }
        public bool R3 { get; set; }

        //Shoulder buttons
        public bool L1 { get; set; }
        public bool L2 { get; set; }
        public bool L { get { return L1; } set { L1 = value; } }
        public bool R1 { get; set; }
        public bool R2 { get; set; }
        public bool R { get { return R1; } set { R1 = value; } }

        //Meta
        public bool active { get; set; }
        private string _macro = string.Empty;
        public string Macro { get { return _macro; } set { _macro = value.ToLowerInvariant(); } }
        public bool Hold { get; set; }
        public bool WasHold => Hold || Math.Round(Sleep_Frames) < 0;
        public double Held_Frames { get; set; }
        public double Sleep_Frames { get; set; }
        public double Expire_Frames { get; set; } = 0;
        public bool IsExpired => Math.Round(Expire_Frames) <= 0;
        public IEnumerable<TPPInput> Series;

        public bool IsEmpty {
            get {
                if ((Series ?? new List<TPPInput>()).Any()) return false;
                if (!string.IsNullOrWhiteSpace(Macro)) return false;
                var ctrl = ToX360();
                if (ctrl.Buttons != X360Buttons.None) return false;
                if (ctrl.LeftStickX + ctrl.LeftStickY + ctrl.LeftTrigger + ctrl.RightStickX + ctrl.RightStickY + ctrl.RightTrigger != 0) return false;
                return true;
            }
        }

        public virtual X360Controller ToX360()
        {
            X360Controller controller = new X360Controller();

            if (Up) controller.Buttons |= X360Buttons.Up;
            if (Down) controller.Buttons |= X360Buttons.Down;
            if (Left) controller.Buttons |= X360Buttons.Left;
            if (Right) controller.Buttons |= X360Buttons.Right;

            if (A || Cross) controller.Buttons |= X360Buttons.A;
            if (B || Circle) controller.Buttons |= X360Buttons.B;
            if (X || Square) controller.Buttons |= X360Buttons.X;
            if (Y || Triangle) controller.Buttons |= X360Buttons.Y;

            if (Start) controller.Buttons |= X360Buttons.Start;
            if (Select) controller.Buttons |= X360Buttons.Back;

            if (Lup) controller.LeftStickY = short.MaxValue;
            if (Ldown) controller.LeftStickY = short.MinValue;
            if (Lleft) controller.LeftStickX = short.MinValue;
            if (Lright) controller.LeftStickX = short.MaxValue;
            if (L3) controller.Buttons |= X360Buttons.LeftStick;
            if (Rup) controller.RightStickY = short.MaxValue;
            if (Rdown) controller.RightStickY = short.MinValue;
            if (Rleft) controller.RightStickX = short.MinValue;
            if (Rright) controller.RightStickX = short.MaxValue;
            if (R3) controller.Buttons |= X360Buttons.RightStick;

            if (L1) controller.Buttons |= X360Buttons.LeftBumper;
            if (L2) controller.LeftTrigger = byte.MaxValue;
            if (R1) controller.Buttons |= X360Buttons.RightBumper;
            if (R2) controller.RightTrigger = byte.MaxValue;

            return controller;
        }
    }
}
