using System;
using System.Runtime.InteropServices;

namespace Glib.Win32
{
    internal class Win32Methods
    {
        [DllImport("user32.dll")]
        internal static extern IntPtr CreateWindowEx(int dwExStyle, string lpClassName, string lpWindowName, uint dwStyle, int x, int y, int nWidth, int nHeight,
                                                     IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [DllImport("user32.dll")]
        internal static extern IntPtr DispatchMessage([In] ref MSG msg);

        [DllImport("user32.dll")]
        internal static extern short GetAsyncKeyState([In] int vKey);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        internal static extern uint GetDoubleClickTime();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool PeekMessage(out MSG msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);

        [DllImport("user32.dll")]
        internal static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("user32.dll")]
        internal static extern bool TranslateMessage([In] ref MSG msg);
    }
}