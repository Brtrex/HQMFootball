using HockeyEditor;
using System;
using System.Diagnostics;

namespace FootballGame
{
    public class Football
    {
        static readonly int roundnum = GameInfo.Period;
        static bool highlighted = false;
        bool displaylabel = false;
        public static int WarmupTime
        {
            get { return MemoryEditor.ReadInt(0x07D33DA0); }
            set { MemoryEditor.WriteInt(value, 0x07D33DA0); }
        }
        public static void Highlight(int value) //0=disable 1=red 2=blue
        {
            int shape_a = 0x0043585D;
            int shape_h = 0x004358EA;
            int shape_b = 0x004355B9;
            int color = 0x004355BE;
            int color_change = 0x04600081;
            int y = 0x07D3490C;

            if (value == 0 && highlighted)
            {
                Debug.Print("disable highlight");
                MemoryEditor.WriteBytes(new byte[2] { 0x6A, 0x04}, shape_a);
                MemoryEditor.WriteBytes(new byte[7] { 0xC7, 0x45, 0xF0, 0x00, 0x00, 0x80, 0x3C }, shape_h);
                MemoryEditor.WriteBytes(new byte[5] { 0x68, 0x00, 0x00, 0x80, 0x3E }, shape_b);
                MemoryEditor.WriteBytes(new byte[6] { 0x6A, 0x00, 0x6A, 0x00, 0x6A, 0x00 }, color);
                MemoryEditor.WriteFloat(352f, y);
                System.Threading.Thread.Sleep(1000);
                highlighted = false;
            }

            if (value == 1 && !highlighted)
            {
                highlighted = true;
                Debug.Print("red highlight");
                MemoryEditor.WriteBytes(new byte[2] { 0x6A, 0x06}, shape_a);
                MemoryEditor.WriteBytes(new byte[7] { 0xC7, 0x45, 0xF0, 0x00, 0x00, 0x80, 0x3F }, shape_h);
                MemoryEditor.WriteBytes(new byte[5] { 0x68, 0x00, 0x00, 0x80, 0x3F }, shape_b);
                MemoryEditor.WriteBytes(new byte[6] { 0xE9, 0xBE, 0xAA, 0x1C, 0x04, 0x90 }, color);
                MemoryEditor.WriteBytes(new byte[14] { 0x6A, 0x00, 0x6A, 0x00, 0x68, 0x00, 0x00, 0x80, 0x3F, 0xE9, 0x35, 0x55, 0xE3, 0xFB }, color_change);
                MemoryEditor.WriteFloat(1352f, y);
            }
            if (value == 2 && !highlighted)
            {
                highlighted = true;
                Debug.Print("blue highlight");
                MemoryEditor.WriteBytes(new byte[2] { 0x6A, 0x06}, shape_a);
                MemoryEditor.WriteBytes(new byte[7] { 0xC7, 0x45, 0xF0, 0x00, 0x00, 0x80, 0x3F }, shape_h);
                MemoryEditor.WriteBytes(new byte[5] { 0x68, 0x00, 0x00, 0x80, 0x3F }, shape_b);
                MemoryEditor.WriteBytes(new byte[6] { 0xE9, 0xBE, 0xAA, 0x1C, 0x04, 0x90 }, color);
                MemoryEditor.WriteBytes(new byte[14] { 0x68, 0x00, 0x00, 0x80, 0x3F, 0x6A, 0x00, 0x6A, 0x00, 0xE9, 0x35, 0x55, 0xE3, 0xFB }, color_change);
                MemoryEditor.WriteFloat(1352f, y);
            }
        }
        public static float Distance2D(HQMVector v1, HQMVector v2)
        {
            return ((v1.X - v2.X) * (v1.X - v2.X) + (v1.Z - v2.Z) * (v1.Z - v2.Z));
        }
        public void Detection()
        {
            FaceOffDetection();
            if (!highlighted)
            {
                if (WarmupTime < 3500 && WarmupTime > 3000)
                {
                    Highlight(2);
                }
                else if (WarmupTime < 4500 && WarmupTime > 4000)
                {
                    Highlight(1);
                }
                else if(WarmupTime < 5500 && WarmupTime > 5000)
                {
                    Highlight(1);
                }
                else if(WarmupTime < 6500 && WarmupTime > 6000)
                {
                    Highlight(2);
                }
                else if(WarmupTime < 7500 && WarmupTime > 7000)
                {
                    Highlight(2);
                }
                else if(WarmupTime < 8500 && WarmupTime > 8000)
                {
                    Highlight(1);
                }
            }
            else
            {
                if (WarmupTime == 0)
                {
                    Highlight(0);
                }
            }
        }
        public void FaceOffDetection()
        {
            if (GameInfo.Period != roundnum && !GameInfo.IsGameOver && !displaylabel)
                {
                if (GameInfo.Period == 2)
                {
                    Debug.Print("halftime");
                    MemoryEditor.WriteString(0x07D34968, "Half-time");
                    MemoryEditor.WriteString(0x07D348E0, "HT");
                    displaylabel = true;
                }
                if (GameInfo.Period == 3)
                {
                    Debug.Print("extratime");
                    MemoryEditor.WriteString(0x07D34968, "Extra-time");
                    MemoryEditor.WriteString(0x07D348E0, "ET");
                    displaylabel = true;
                }
                if (GameInfo.Period == 4)
                {
                    Debug.Print("game over?");
                    displaylabel = true;
                }
            }
            if (GameInfo.IntermissionTime == 0 && displaylabel)
            {
                displaylabel = false;
            }
        }
    }
}
