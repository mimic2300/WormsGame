﻿using System;
using System.Runtime.InteropServices;

namespace Glib.Win32
{
    /// <summary>
    /// Struct for native Win32 messages.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct MSG
    {
        public IntPtr hWnd;
        public int msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point p;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        public int X;
        public int Y;
    }
}