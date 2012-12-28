using Glib.Win32;
using SharpDX.Toolkit;
using System;
using System.Drawing;

namespace Glib.Input
{
    /// <summary>
    /// Získá informace o myši.
    /// </summary>
    public class Mouse
    {
        #region Proměnné

        const int ButtonCount = 5;

        GlibWindow window;

        Point position;
        Point oldPosition;

        bool[] buttons;
        bool[] oldButtons;

        MouseButtonsEnum? lastClickedButton;
        TimeSpan elapsedSinceClick;
        MouseButtonsEnum? doubleClickedButton;

        #endregion

        #region Vlastnosti

        /// <summary>
        /// Returns true if the mouse is within the game window client area.
        /// </summary>
        public bool IsWithinDisplayArea
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns the double click rate for the mouse.
        /// </summary>
        public TimeSpan DoubleClickRate
        {
            get;
            private set;
        }

        /// <summary>
        /// The position of the cursor.
        /// </summary>
        public Point Position
        {
            get { return position; }
        }

        /// <summary>
        /// The delta of the position from the last update.
        /// </summary>
        public Point PositionDelta
        {
            get { return new Point(position.X - oldPosition.X, position.Y - oldPosition.Y); }
        }

        /// <summary>
        /// The X position of the cursor.
        /// </summary>
        public int X
        {
            get { return position.X; }
        }

        /// <summary>
        /// The Y position of the cursor.
        /// </summary>
        public int Y
        {
            get { return position.Y; }
        }

        /// <summary>
        /// Returns true if the left mouse button is currently down.
        /// </summary>
        public bool LeftButton
        {
            get { return buttons[(int)MouseButtonsEnum.Left]; }
        }

        /// <summary>
        /// Returns true if the right mouse button is currently down.
        /// </summary>
        public bool RightButton
        {
            get { return buttons[(int)MouseButtonsEnum.Right]; }
        }

        /// <summary>
        /// Returns true if the middle mouse button is currently down.
        /// </summary>
        public bool MiddleButton
        {
            get { return buttons[(int)MouseButtonsEnum.Middle]; }
        }

        /// <summary>
        /// Returns true if XButton1 mouse button is currently down.
        /// </summary>
        public bool XButton1
        {
            get { return buttons[(int)MouseButtonsEnum.XButton1]; }
        }

        /// <summary>
        /// Returns true if XButton2 mouse button is currently down.
        /// </summary>
        public bool XButton2
        {
            get { return buttons[(int)MouseButtonsEnum.XButton2]; }
        }

        #endregion

        #region Konstruktory

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="window"></param>
        public Mouse(GlibWindow window)
        {
            if (window == null)
                throw new ArgumentNullException("window", "Window was null. Please provide an instance of GlibWindow.");

            this.window = window;

            DoubleClickRate = TimeSpan.FromMilliseconds(Win32Methods.GetDoubleClickTime());

            position = new Point(0, 0);
            oldPosition = new Point(0, 0);

            buttons = new bool[ButtonCount];
            oldButtons = new bool[ButtonCount];

            window.OnUpdate += Update;
        }

        #endregion

        #region Metody

        /// <summary>
        /// Updates the state of the mouse.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            IntPtr windowHandle = ((System.Windows.Forms.Form)window.Window.NativeWindow).Handle;

            Win32Point point;
            Win32Methods.GetCursorPos(out point);
            Win32Methods.ScreenToClient(windowHandle, ref point);

            UpdateIsWithinDisplayArea(point);

            oldPosition = position;
            position = new Point(point.X, point.Y);

            Array.Copy(buttons, oldButtons, ButtonCount);

            buttons[(int)MouseButtonsEnum.Left] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_LBUTTON) != 0);
            buttons[(int)MouseButtonsEnum.Right] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_RBUTTON) != 0);
            buttons[(int)MouseButtonsEnum.Middle] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_MBUTTON) != 0);
            buttons[(int)MouseButtonsEnum.XButton1] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_XBUTTON1) != 0);
            buttons[(int)MouseButtonsEnum.XButton2] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_XBUTTON2) != 0);

            DoubleClickDetection(gameTime);
        }

        private void DoubleClickDetection(GameTime gameTime)
        {
            doubleClickedButton = null;

            if (lastClickedButton != null)
            {
                elapsedSinceClick += gameTime.ElapsedGameTime;

                if (elapsedSinceClick > DoubleClickRate ||
                    elapsedSinceClick > TimeSpan.FromSeconds(5))
                {
                    lastClickedButton = null;
                }
            }

            MouseButtonsEnum? clickedButton = null;

            for (int i = 0; i < ButtonCount; i++)
            {
                if (IsButtonPressed((MouseButtonsEnum)i))
                {
                    clickedButton = (MouseButtonsEnum)i;
                }
            }

            if (clickedButton != null)
            {
                if (clickedButton.Value == lastClickedButton)
                {
                    if (elapsedSinceClick <= DoubleClickRate)
                    {
                        doubleClickedButton = clickedButton;
                        lastClickedButton = null;
                        elapsedSinceClick = TimeSpan.Zero;
                    }
                }
                else
                {
                    lastClickedButton = clickedButton;
                    elapsedSinceClick = TimeSpan.Zero;
                }
            }
        }

        /// <summary>
        /// Aktualizuje, jestli je kursor vne nebo vevnitr okna.
        /// </summary>
        /// <param name="point"></param>
        private void UpdateIsWithinDisplayArea(Win32Point point)
        {
            IsWithinDisplayArea = point.X >= 0 && point.Y >= 0 && point.X <= window.Window.ClientBounds.Width && point.Y <= window.Window.ClientBounds.Height;
        }

        /// <summary>
        /// Returns true if the given mouse button is currently down.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsButtonDown(MouseButtonsEnum button)
        {
            return buttons[(int)button];
        }

        /// <summary>
        /// Returns true if the given mouse button is currently down and was not on the last update. Method is same as IsButtonClicked().
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsButtonPressed(MouseButtonsEnum button)
        {
            return buttons[(int)button] && !oldButtons[(int)button];
        }

        /// <summary>
        /// Returns true if the given mouse button is clicked and was clicked twice within the time span specified by DoubleClickRate.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsButtonDoubleClicked(MouseButtonsEnum button)
        {
            return doubleClickedButton != null && doubleClickedButton.Value == button;
        }

        /// <summary>
        /// Resets double click tracking for the mouse.
        /// </summary>
        public void ResetDoubleClick()
        {
            doubleClickedButton = null;
            lastClickedButton = null;
            elapsedSinceClick = TimeSpan.Zero;
        }

        /// <summary>
        /// Returns true if the given mouse button is currently up.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsButtonUp(MouseButtonsEnum button)
        {
            return !buttons[(int)button];
        }

        /// <summary>
        /// Returns true if the given mouse button is currently up and was not up on the last update.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsButtonReleased(MouseButtonsEnum button)
        {
            return !buttons[(int)button] && oldButtons[(int)button];
        }

        /// <summary>
        /// Prekryje metodu ToString.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string downButtons = "";

            for (int i = 0; i < ButtonCount; i++)
            {
                if (IsButtonDown((MouseButtonsEnum)i))
                {
                    downButtons += " " + ((MouseButtonsEnum)i).ToString();
                }
            }

            return String.Format("[{0}, {1}]\nIsIn: {2} DownButtons:{3}", position.X, position.Y, IsWithinDisplayArea, downButtons);
        }

        #endregion
    }
}