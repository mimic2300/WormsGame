using System;
using System.Runtime.InteropServices;

namespace Glib.Native
{
    /// <summary>
    /// Poskytuje funkce z WinAPI.
    /// </summary>
    public class W32
    {
        /// <summary>
        /// Převede lParam na hodnotu.
        /// </summary>
        /// <param name="lParam">lParam</param>
        /// <returns></returns>
        public static ushort LowWord(uint lParam)
        {
            return (ushort)lParam;
        }

        /// <summary>
        /// Převedel wParam na hodnotu.
        /// </summary>
        /// <param name="wParam">wParam</param>
        /// <returns></returns>
        public static ushort HighWord(uint wParam)
        {
            return (ushort)(wParam >> 16);
        }

        /// <summary>
        /// Creates an overlapped, pop-up, or child window with an extended window style; otherwise,
        /// this function is identical to the CreateWindow function.
        /// </summary>
        /// <param name="dwExStyle">The extended window style of the window being created.</param>
        /// <param name="lpClassName">A null-terminated string or a class atom created by a previous call to the RegisterClass or RegisterClassEx function. The atom must be in the low-order word of lpClassName; the high-order word must be zero. If lpClassName is a string, it specifies the window class name. The class name can be any name registered with RegisterClass or RegisterClassEx, provided that the module that registers the class is also the module that creates the window. The class name can also be any of the predefined system class names.</param>
        /// <param name="lpWindowName">The window name. If the window style specifies a title bar, the window title pointed to by lpWindowName is displayed in the title bar. When using CreateWindow to create controls, such as buttons, check boxes, and static controls, use lpWindowName to specify the text of the control. When creating a static control with the SS_ICON style, use lpWindowName to specify the icon name or identifier. To specify an identifier, use the syntax "#num".</param>
        /// <param name="dwStyle">The style of the window being created.</param>
        /// <param name="x">The initial horizontal position of the window. For an overlapped or pop-up window, the x parameter is the initial x-coordinate of the window's upper-left corner, in screen coordinates. For a child window, x is the x-coordinate of the upper-left corner of the window relative to the upper-left corner of the parent window's client area. If x is set to CW_USEDEFAULT, the system selects the default position for the window's upper-left corner and ignores the y parameter. CW_USEDEFAULT is valid only for overlapped windows; if it is specified for a pop-up or child window, the x and y parameters are set to zero.</param>
        /// <param name="y">The initial vertical position of the window. For an overlapped or pop-up window, the y parameter is the initial y-coordinate of the window's upper-left corner, in screen coordinates. For a child window, y is the initial y-coordinate of the upper-left corner of the child window relative to the upper-left corner of the parent window's client area. For a list box y is the initial y-coordinate of the upper-left corner of the list box's client area relative to the upper-left corner of the parent window's client area.</param>
        /// <param name="nWidth">The width, in device units, of the window. For overlapped windows, nWidth is the window's width, in screen coordinates, or CW_USEDEFAULT. If nWidth is CW_USEDEFAULT, the system selects a default width and height for the window; the default width extends from the initial x-coordinates to the right edge of the screen; the default height extends from the initial y-coordinate to the top of the icon area. CW_USEDEFAULT is valid only for overlapped windows; if CW_USEDEFAULT is specified for a pop-up or child window, the nWidth and nHeight parameter are set to zero.</param>
        /// <param name="nHeight">The height, in device units, of the window. For overlapped windows, nHeight is the window's height, in screen coordinates. If the nWidth parameter is set to CW_USEDEFAULT, the system ignores nHeight.</param>
        /// <param name="hWndParent">A handle to the parent or owner window of the window being created. To create a child window or an owned window, supply a valid window handle. This parameter is optional for pop-up windows.</param>
        /// <param name="hMenu">A handle to a menu, or specifies a child-window identifier, depending on the window style. For an overlapped or pop-up window, hMenu identifies the menu to be used with the window; it can be NULL if the class menu is to be used. For a child window, hMenu specifies the child-window identifier, an integer value used by a dialog box control to notify its parent about events. The application determines the child-window identifier; it must be unique for all child windows with the same parent window.</param>
        /// <param name="hInstance">A handle to the instance of the module to be associated with the window.</param>
        /// <param name="lpParam">Pointer to a value to be passed to the window through the CREATESTRUCT structure (lpCreateParams member) pointed to by the lParam param of the WM_CREATE message. This message is sent to the created window by this function before it returns.</param>
        /// <returns>If the function succeeds, the return value is a handle to the new window.</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr CreateWindowEx(
            [In] int dwExStyle,
            [In, Optional] string lpClassName,
            [In, Optional] string lpWindowName,
            [In] uint dwStyle,
            [In] int x,
            [In] int y,
            [In] int nWidth,
            [In] int nHeight,
            [In, Optional] IntPtr hWndParent,
            [In, Optional] IntPtr hMenu,
            [In, Optional] IntPtr hInstance,
            [In, Optional] IntPtr lpParam);

        /// <summary>
        /// Dispatches a message to a window procedure. It is typically used to dispatch a message retrieved by the GetMessage function.
        /// </summary>
        /// <param name="lpmsg">A pointer to a structure that contains the message.</param>
        /// <returns>The return value specifies the value returned by the window procedure. Although its meaning depends on the message being dispatched, the return value generally is ignored.</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage(
            [In] ref MSG lpmsg);

        /// <summary>
        /// Determines whether a key is up or down at the time the function is called, and whether the key was pressed after a previous call to GetAsyncKeyState.
        /// </summary>
        /// <param name="vKey">The virtual-key code. For more information.</param>
        /// <returns>If the function succeeds, the return value specifies whether the key was pressed since the last call to GetAsyncKeyState, and whether the key is currently up or down. If the most significant bit is set, the key is down, and if the least significant bit is set, the key was pressed after the previous call to GetAsyncKeyState. However, you should not rely on this last behavior.</returns>
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(
            [In] int vKey);

        /// <summary>
        /// Retrieves the position of the mouse cursor, in screen coordinates.
        /// </summary>
        /// <param name="lpPoint">A pointer to a POINT structure that receives the screen coordinates of the cursor.</param>
        /// <returns>Returns nonzero if successful or zero otherwise.</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(
            [Out] out POINT lpPoint);

        /// <summary>
        /// Retrieves the current double-click time for the mouse. A double-click is a series of two clicks of the mouse button, the second occurring within a specified time after the first. The double-click time is the maximum number of milliseconds that may occur between the first and second click of a double-click. The maximum double-click time is 5000 milliseconds.
        /// </summary>
        /// <returns>The return value specifies the current double-click time, in milliseconds. The maximum return value is 5000 milliseconds.</returns>
        [DllImport("user32.dll")]
        public static extern uint GetDoubleClickTime();

        /// <summary>
        /// Copies the status of the 256 virtual keys to the specified buffer.
        /// </summary>
        /// <param name="lpKeyState">The 256-byte array that receives the status data for each virtual key.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetKeyboardState(
            [Out] byte[] lpKeyState);

        /// <summary>
        /// Dispatches incoming sent messages, checks the thread message queue for a posted message, and retrieves the message (if any exist).
        /// </summary>
        /// <param name="lpMsg">A pointer to an MSG structure that receives message information.</param>
        /// <param name="hWnd">A handle to the window whose messages are to be retrieved. The window must belong to the current thread.</param>
        /// <param name="wMsgFilterMin">The value of the first message in the range of messages to be examined. Use WM_KEYFIRST (0x0100) to specify the first keyboard message or WM_MOUSEFIRST (0x0200) to specify the first mouse message.</param>
        /// <param name="wMsgFilterMax">The value of the last message in the range of messages to be examined. Use WM_KEYLAST to specify the last keyboard message or WM_MOUSELAST to specify the last mouse message.</param>
        /// <param name="wRemoveMsg">Specifies how messages are to be handled.</param>
        /// <returns>If a message is available, the return value is nonzero. If no messages are available, the return value is zero.</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PeekMessage(
            [Out] out MSG lpMsg,
            [In, Optional] IntPtr hWnd,
            [In] uint wMsgFilterMin,
            [In] uint wMsgFilterMax,
            [In] PeakMessage wRemoveMsg); // uint

        /// <summary>
        /// The ScreenToClient function converts the screen coordinates of a specified point on the screen to client-area coordinates.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose client area will be used for the conversion.</param>
        /// <param name="lpPoint">A pointer to a POINT structure that specifies the screen coordinates to be converted.</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.</returns>
        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(
            [In] IntPtr hWnd,
            ref POINT lpPoint);

        /// <summary>
        /// Translates virtual-key messages into character messages. The character messages are posted to the calling thread's message queue, to be read the next time the thread calls the GetMessage or PeekMessage function.
        /// </summary>
        /// <param name="lpMsg">A pointer to an MSG structure that contains message information retrieved from the calling thread's message queue by using the GetMessage or PeekMessage function.</param>
        /// <returns>If the message is translated (that is, a character message is posted to the thread's message queue), the return value is nonzero.</returns>
        [DllImport("user32.dll")]
        public static extern bool TranslateMessage(
            [In] ref MSG lpMsg);
    }
}
