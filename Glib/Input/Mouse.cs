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

        /// <summary>
        /// Počet tlačítek na myši.
        /// </summary>
        public const int BUTTON_COUNT = 5;

        private GlibWindow window;
        private Point position;
        private Point oldPosition;
        private bool[] buttons;
        private bool[] oldButtons;
        private MouseButtonsType? lastClickedButton;
        private MouseButtonsType? doubleClickedButton;
        private TimeSpan elapsedSinceClick;

        #endregion

        #region Vlastnosti

        /// <summary>
        /// Pokud se myš nachází v okně.
        /// </summary>
        public bool IsWithinDisplayArea { get; private set; }

        /// <summary>
        /// Rychlost dvoj-kliku.
        /// </summary>
        public TimeSpan DoubleClickRate { get; private set; }

        /// <summary>
        /// Pozice myši.
        /// </summary>
        public Point Position
        {
            get { return position; }
        }

        /// <summary>
        /// Delta pozice od poslední aktualizace.
        /// </summary>
        public Point PositionDelta
        {
            get { return new Point(position.X - oldPosition.X, position.Y - oldPosition.Y); }
        }

        /// <summary>
        /// Pozice X.
        /// </summary>
        public int X
        {
            get { return position.X; }
        }

        /// <summary>
        /// Pozice Y.
        /// </summary>
        public int Y
        {
            get { return position.Y; }
        }

        /// <summary>
        /// Levé tlačítko je stisknuto.
        /// </summary>
        public bool LeftButton
        {
            get { return buttons[(int)MouseButtonsType.Left]; }
        }

        /// <summary>
        /// Pravé tlačítko je stisknuto.
        /// </summary>
        public bool RightButton
        {
            get { return buttons[(int)MouseButtonsType.Right]; }
        }

        /// <summary>
        /// Prostřední tlačítko je stisknuto.
        /// </summary>
        public bool MiddleButton
        {
            get { return buttons[(int)MouseButtonsType.Middle]; }
        }

        /// <summary>
        /// X1 tlačítko je stisknuto.
        /// </summary>
        public bool XButton1
        {
            get { return buttons[(int)MouseButtonsType.XButton1]; }
        }

        /// <summary>
        /// X2 tlačítko je stisknuto.
        /// </summary>
        public bool XButton2
        {
            get { return buttons[(int)MouseButtonsType.XButton2]; }
        }

        #endregion

        #region Konstruktory

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="window"></param>
        public Mouse(GlibWindow window)
        {
            if (window == null)
                throw new ArgumentNullException("window", "Window was null. Please provide an instance of GlibWindow.");

            this.window = window;

            DoubleClickRate = TimeSpan.FromMilliseconds(Win32Methods.GetDoubleClickTime());

            position = Point.Empty;
            oldPosition = Point.Empty;

            buttons = new bool[BUTTON_COUNT];
            oldButtons = new bool[BUTTON_COUNT];

            window.OnUpdate += Update;
        }

        #endregion

        #region Funkce

        /// <summary>
        /// Aktualizuje stav myši.
        /// </summary>
        /// <param name="time">Herní čas.</param>
        public void Update(GameTime time)
        {
            IntPtr windowHandle = window.Form.Handle;
            POINT point;
            Win32Methods.GetCursorPos(out point);
            Win32Methods.ScreenToClient(windowHandle, ref point);

            UpdateIsWithinDisplayArea(point);

            oldPosition = position;
            position = new Point(point.X, point.Y);

            Array.Copy(buttons, oldButtons, BUTTON_COUNT);

            buttons[(int)MouseButtonsType.Left] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_LBUTTON) != 0);
            buttons[(int)MouseButtonsType.Right] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_RBUTTON) != 0);
            buttons[(int)MouseButtonsType.Middle] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_MBUTTON) != 0);
            buttons[(int)MouseButtonsType.XButton1] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_XBUTTON1) != 0);
            buttons[(int)MouseButtonsType.XButton2] = (Win32Methods.GetAsyncKeyState(Win32Constants.VK_XBUTTON2) != 0);

            DoubleClickDetection(time);
        }

        /// <summary>
        /// Detekuje dvoj-klik.
        /// </summary>
        /// <param name="time">Herní čas.</param>
        private void DoubleClickDetection(GameTime time)
        {
            doubleClickedButton = null;

            if (lastClickedButton != null)
            {
                elapsedSinceClick += time.ElapsedGameTime;

                if (elapsedSinceClick > DoubleClickRate ||
                    elapsedSinceClick > TimeSpan.FromSeconds(5))
                {
                    lastClickedButton = null;
                }
            }

            MouseButtonsType? clickedButton = null;

            for (int i = 0; i < BUTTON_COUNT; i++)
            {
                if (IsButtonPressed((MouseButtonsType)i))
                    clickedButton = (MouseButtonsType)i;
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
        /// Aktualizuje pozici myši, zda se nachází v okně nebo ne.
        /// </summary>
        /// <param name="point">Pozice myši.</param>
        private void UpdateIsWithinDisplayArea(POINT point)
        {
            IsWithinDisplayArea = (point.X >= 0 && point.Y >= 0 && point.X <= window.Window.ClientBounds.Width && point.Y <= window.Window.ClientBounds.Height);
        }

        /// <summary>
        /// Pokud se stisklo tlačítko myši.
        /// </summary>
        /// <param name="button">Jaké tlačítko se má zjistit.</param>
        /// <returns>Vrací true, pokud je stisknuto.</returns>
        public bool IsButtonDown(MouseButtonsType button)
        {
            return buttons[(int)button];
        }

        /// <summary>
        /// Pokud se stisklo tlačítko, ale při poslední aktualizaci ještě nebylo stisknuto.
        /// </summary>
        /// <param name="button">Jaké tlačítko se má zjistit.</param>
        /// <returns>Vrací true, pokud je stisknuto.</returns>
        public bool IsButtonPressed(MouseButtonsType button)
        {
            return (buttons[(int)button] && !oldButtons[(int)button]);
        }

        /// <summary>
        /// Pokud byl dvoj-klik na tlačítku.
        /// </summary>
        /// <param name="button">Jaké tlačítko se má zjistit.</param>
        /// <returns>Vrací true, pokud byl dvoj-klik.</returns>
        public bool IsButtonDoubleClicked(MouseButtonsType button)
        {
            return (doubleClickedButton != null && doubleClickedButton.Value == button);
        }

        /// <summary>
        /// Resetuje dvoj-klik myši.
        /// </summary>
        public void ResetDoubleClick()
        {
            doubleClickedButton = null;
            lastClickedButton = null;
            elapsedSinceClick = TimeSpan.Zero;
        }

        /// <summary>
        /// Pokud tlačítko je uvolněno.
        /// </summary>
        /// <param name="button">Jaké tlačítko se má zjistit.</param>
        /// <returns>Vrací true, pokud je uvolněno.</returns>
        public bool IsButtonUp(MouseButtonsType button)
        {
            return !buttons[(int)button];
        }

        /// <summary>
        /// Pokud tlačítko je bylo uvolněni o v předchozí aktualizaci.
        /// </summary>
        /// <param name="button">Jaké tlačítko se má zjistit.</param>
        /// <returns>Vrací true, pokud tlačítko nebylo stisknuto.</returns>
        public bool IsButtonReleased(MouseButtonsType button)
        {
            return !buttons[(int)button] && oldButtons[(int)button];
        }

        /// <summary>
        /// Prekryje metodu ToString.
        /// </summary>
        /// <returns>Vrací výstup myši.</returns>
        public override string ToString()
        {
            string downButtons = null;

            for (int i = 0; i < BUTTON_COUNT; i++)
            {
                if (IsButtonDown((MouseButtonsType)i))
                    downButtons += " " + ((MouseButtonsType)i).ToString();
            }

            if (downButtons == null)
                downButtons = "None";

            return String.Format("[{0}, {1}] IsIn: {2} DownButtons:{3}", position.X, position.Y, IsWithinDisplayArea, downButtons);
        }

        #endregion Funkce
    }
}