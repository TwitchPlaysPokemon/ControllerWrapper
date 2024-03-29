﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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

        public bool Back { get; set; }
        public bool Select { get => Back; set => Back = value; }
        public bool Start { get; set; }

        //Analog sticks
        [JsonConverter(typeof(BoolOrDoubleConverter))]
        public double Lup { get; set; }
        [JsonConverter(typeof(BoolOrDoubleConverter))]
        public double Ldown { get; set; }
        [JsonConverter(typeof(BoolOrDoubleConverter))]
        public double Lleft { get; set; }
        [JsonConverter(typeof(BoolOrDoubleConverter))]
        public double Lright { get; set; }
        public bool L3 { get; set; }
        [JsonConverter(typeof(BoolOrDoubleConverter))]
        public double Aup { get => Lup; set => Lup = value; }
        [JsonConverter(typeof(BoolOrDoubleConverter))]
        public double Adown { get => Ldown; set => Ldown = value; }
        [JsonConverter(typeof(BoolOrDoubleConverter))]
        public double Aleft { get => Lleft; set => Lleft = value; }
        [JsonConverter(typeof(BoolOrDoubleConverter))]
        public double Aright { get => Lright; set => Lright = value; }
        public static double LThrowGlobalMult { get; set; } = 1;

        [JsonConverter(typeof(BoolOrDoubleConverter))]
        public double Rup { get; set; }
        [JsonConverter(typeof(BoolOrDoubleConverter))]
        public double Rdown { get; set; }
        [JsonConverter(typeof(BoolOrDoubleConverter))]
        public double Rleft { get; set; }
        [JsonConverter(typeof(BoolOrDoubleConverter))]
        public double Rright { get; set; }
        public bool R3 { get; set; }
        [JsonConverter(typeof(BoolOrDoubleConverter))]
        public double Cup { get => Rup; set => Rup = value; }
        [JsonConverter(typeof(BoolOrDoubleConverter))]
        public double Cdown { get => Rdown; set => Rdown = value; }
        [JsonConverter(typeof(BoolOrDoubleConverter))]
        public double Cleft { get => Rleft; set => Rleft = value; }
        [JsonConverter(typeof(BoolOrDoubleConverter))]
        public double Cright { get => Rright; set => Rright = value; }
        public static double RThrowGlobalMult { get; set; } = 1;

        //Shoulder buttons
        public bool L1 { get; set; }
        public bool L2 { get; set; }
        public bool L { get => L1; set => L1 = value; }
        public bool LB { get => L1; set => L1 = value; }
        public bool LT { get => L2; set => L2 = value; }
        public bool R1 { get; set; }
        public bool R2 { get; set; }
        public bool R { get => R1; set => R1 = value; }
        public bool Z { get => R1; set => R1 = value; } //GameCube
        public bool RB { get => R1; set => R1 = value; }
        public bool RT { get => R2; set => R2 = value; }

        public static bool ToggleTriggers = false;
        public static bool LTSet = false;
        public static bool RTSet = false;


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

        public bool IsEmpty
        {
            get
            {
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
            if (Back) controller.Buttons |= X360Buttons.Back;

            if (Lup > 0) controller.LeftStickY = (short)(short.MaxValue * Math.Min(Lup, 1) * LThrowGlobalMult);
            if (Ldown > 0) controller.LeftStickY = (short)(short.MinValue * Math.Min(Ldown, 1) * LThrowGlobalMult);
            if (Lleft > 0) controller.LeftStickX = (short)(short.MinValue * Math.Min(Lleft, 1) * LThrowGlobalMult);
            if (Lright > 0) controller.LeftStickX = (short)(short.MaxValue * Math.Min(Lright, 1) * LThrowGlobalMult);
            if (L3) controller.Buttons |= X360Buttons.LeftStick;
            if (Rup > 0) controller.RightStickY = (short)(short.MaxValue * Math.Min(Rup, 1) * RThrowGlobalMult);
            if (Rdown > 0) controller.RightStickY = (short)(short.MinValue * Math.Min(Rdown, 1) * RThrowGlobalMult);
            if (Rleft > 0) controller.RightStickX = (short)(short.MinValue * Math.Min(Rleft, 1) * RThrowGlobalMult);
            if (Rright > 0) controller.RightStickX = (short)(short.MaxValue * Math.Min(Rright, 1) * RThrowGlobalMult);
            if (R3) controller.Buttons |= X360Buttons.RightStick;

            if (L1) controller.Buttons |= X360Buttons.LeftBumper;
            if (R1) controller.Buttons |= X360Buttons.RightBumper;

            if (ToggleTriggers)
            {
                if (LT)
                {
                    RTSet = false;
                    LTSet = !LTSet;
                }
                if (RT)
                {
                    LTSet = false;
                    RTSet = !RTSet;
                }
                if (LT && RT)
                {
                    LTSet = RTSet = false;
                }
                if (LTSet) controller.LeftTrigger = byte.MaxValue;
                if (RTSet) controller.RightTrigger = byte.MaxValue;
                LT = RT = false; //buttons are handled, don't bounce
            }
            else
            {
                if (L2) controller.LeftTrigger = byte.MaxValue;
                if (R2) controller.RightTrigger = byte.MaxValue;
            }

            return controller;
        }

        public virtual bool OverlapsWith(TPPInput otherInput)
            => otherInput != null && (
               (otherInput.A && A)
            || (otherInput.B && B)
            || (otherInput.X && X)
            || (otherInput.Y && Y)
            || (otherInput.L1 && L1)
            || (otherInput.L2 && L2)
            || (otherInput.L3 && L3)
            || (otherInput.R1 && R1)
            || (otherInput.R2 && R2)
            || (otherInput.R3 && R3)
            || (otherInput.Back && Back)
            || (otherInput.Start && Start)
            || (otherInput.Up && Up)
            || (otherInput.Down && Down)
            || (otherInput.Left && Left)
            || (otherInput.Right && Right)
            || (otherInput.Lup > 0 && Lup > 0)
            || (otherInput.Ldown > 0 && Ldown > 0)
            || (otherInput.Lleft > 0 && Lleft > 0)
            || (otherInput.Lright > 0 && Lright > 0)
            || (otherInput.Rup > 0 && Rup > 0)
            || (otherInput.Rdown > 0 && Rdown > 0)
            || (otherInput.Rleft > 0 && Rleft > 0)
            || (otherInput.Rright > 0 && Rright > 0)
            || (otherInput.Hold && Hold)
            );

        public virtual void MergeWith(TPPInput otherInput)
        {
            if (otherInput != null)
            {
                A = A || otherInput.A;
                B = B || otherInput.B;
                X = X || otherInput.X;
                Y = Y || otherInput.Y;
                L1 = L1 || otherInput.L1;
                L2 = L2 || otherInput.L2;
                L3 = L3 || otherInput.L3;
                R1 = R1 || otherInput.R1;
                R2 = R2 || otherInput.R2;
                R3 = R3 || otherInput.R3;
                Back = Back || otherInput.Back;
                Start = Start || otherInput.Start;
                Up = Up || otherInput.Up;
                Down = Down || otherInput.Down;
                Left = Left || otherInput.Left;
                Right = Right || otherInput.Right;
                Lup = otherInput.Lup > 0 ? otherInput.Lup : Lup;
                Ldown = otherInput.Ldown > 0 ? otherInput.Ldown : Ldown;
                Lleft = otherInput.Lleft > 0 ? otherInput.Lleft : Lleft;
                Lright = otherInput.Lright > 0 ? otherInput.Lright : Lright;
                Rup = otherInput.Rup > 0 ? otherInput.Rup : Rup;
                Rdown = otherInput.Rdown > 0 ? otherInput.Rdown : Rdown;
                Rleft = otherInput.Rleft > 0 ? otherInput.Rleft : Rleft;
                Rright = otherInput.Rright > 0 ? otherInput.Rright : Rright;
            }
        }
    }
}
