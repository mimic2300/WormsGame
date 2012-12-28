using SharpDX.Windows;
using System;
using System.Drawing;
using System.Windows.Forms;

using DrawingSize = SharpDX.DrawingSize;

namespace Glib
{
    /// <summary>
    /// Základní objekt herního okna.
    /// </summary>
    public abstract class GameWindow : IDisposable
    {
        #region Proměnné

        private GameTime mGameTime = null;
        private RenderForm mRenderForm = null;
        // základní parametry herního okna, které lze měnit
        private WindowConfig mWindowParams;
        private double mDeltaTime = 0;
        private int mFps = 0;
        // pouze počítadlo pro FPS
        private int mFpsCount = 0;
        // pouze sčítač delta času
        private double mFpsAccumulator = 0;

        #endregion Proměnné

        #region Konstruktory

        /// <summary>
        /// Hlavní konstruktor.
        /// </summary>
        public GameWindow()
        {
            mGameTime = new GameTime();

            mRenderForm = new RenderForm();
            mRenderForm.Icon = null;
            mRenderForm.Text = "Game Window";
            mRenderForm.FormBorderStyle = FormBorderStyle.Sizable;
            mRenderForm.MaximizeBox = true;
            mRenderForm.Size = new Size(800, 600);
            mRenderForm.StartPosition = FormStartPosition.CenterScreen;

            mRenderForm.KeyDown += new KeyEventHandler((o, e) => { KeyDown(e); });
            mRenderForm.KeyUp += new KeyEventHandler((o, e) => { KeyUp(e); });
            mRenderForm.MouseDown += new MouseEventHandler((o, e) => { MouseDown(e); });
            mRenderForm.MouseUp += new MouseEventHandler((o, e) => { MouseUp(e); });
            mRenderForm.MouseMove += new MouseEventHandler((o, e) => { MouseMove(e); });
            mRenderForm.MouseWheel += new MouseEventHandler((o, e) => { MouseWheel(e); });

            mWindowParams = new WindowConfig(mRenderForm);
        }

        /// <summary>
        /// Konstruktor pro nastavení titulku herního okna.
        /// </summary>
        /// <param name="title">Titulek okna.</param>
        public GameWindow(string title)
            : this()
        {
            mWindowParams.Title = title;
        }

        /// <summary>
        /// Konstruktor pro nastavení titulku a velikosti herního okna.
        /// </summary>
        /// <param name="title">Titulek okna.</param>
        /// <param name="width">Šířka okna.</param>
        /// <param name="height">Výška okna.</param>
        public GameWindow(string title, int width, int height)
            : this(title)
        {
            mWindowParams.Width = width;
            mWindowParams.Height = height;
        }

        #endregion Konstruktory

        #region Vlastnosti

        /// <summary>
        /// Parametry herního okna.
        /// </summary>
        public WindowConfig WindowParams
        {
            get { return mWindowParams; }
        }

        /// <summary>
        /// Získá nebo nastaví titulek okna.
        /// </summary>
        public string WindowTitle
        {
            get { return mWindowParams.Title; }
            set
            {
                mWindowParams.Title = value;
                mRenderForm.Text = value;
            }
        }

        /// <summary>
        /// Handle herního okna.
        /// </summary>
        public IntPtr Handle
        {
            get { return mWindowParams.Handle; }
        }

        /// <summary>
        /// Šířka herního okna.
        /// </summary>
        public int Width
        {
            get { return mWindowParams.Width; }
        }

        /// <summary>
        /// Výška herního okna.
        /// </summary>
        public int Height
        {
            get { return mWindowParams.Height; }
        }

        /// <summary>
        /// FPS.
        /// </summary>
        public int FPS
        {
            get { return mFps; }
        }

        /// <summary>
        /// Čas delta.
        /// </summary>
        public double DeltaTime
        {
            get { return mDeltaTime; }
        }

        #endregion Vlastnosti

        /// <summary>
        /// Uvolní prostředky herního okna.
        /// </summary>
        public void Dispose()
        {
            if (mGameTime.IsRunning)
                mGameTime.Stop();

            mRenderForm.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Ukončí herní okno.
        /// </summary>
        public void Exit()
        {
            mRenderForm.Close();
        }

        /// <summary>
        /// Aplikuje změny, které se provedly v parametrech okna při initializaci.
        /// </summary>
        private void ApplyChanges()
        {
            mRenderForm.Text = mWindowParams.Title;
            mRenderForm.Icon = mWindowParams.Icon;

            int width = Screen.PrimaryScreen.Bounds.Width - mWindowParams.Width;
            int height = Screen.PrimaryScreen.Bounds.Height - mWindowParams.Height;

            mRenderForm.Bounds = new Rectangle(width / 2, height / 2, mWindowParams.Width, mWindowParams.Height);
        }

        /// <summary>
        /// Spustí a zobrazí herní okno.
        /// </summary>
        public void Run()
        {
            WindowConfiguration(ref mWindowParams); // virtual
            Initialize(); // abstract

            bool isResizing = false;
            bool isClosing = false;

            mRenderForm.ResizeBegin += new EventHandler((o, e) => { isResizing = true; });
            mRenderForm.ResizeEnd += new EventHandler((o, e) =>
            {
                isResizing = false;
                mWindowParams.Width = mRenderForm.Width;
                mWindowParams.Height = mRenderForm.Height;
                ResizeEnd(new DrawingSize(mRenderForm.Width, mRenderForm.Height)); // virtual
            });
            mRenderForm.FormClosing += new FormClosingEventHandler((o, e) => { isClosing = true; });

            LoadContent(); // virtual

            mGameTime.Start();

            RenderLoop.Run(mRenderForm, () =>
            {
                if (isClosing)
                    return;

                mDeltaTime = mGameTime.UpdateDeltaTime();

                GameTimeInfo gameTime = new GameTimeInfo(mGameTime.ElapsedTime, mDeltaTime);

                Update(gameTime); // virtual

                mFpsAccumulator += mDeltaTime;
                ++mFpsCount;

                if (mFpsAccumulator >= 1.0)
                {
                    mFps = (int)(mFpsCount / mFpsAccumulator);
                    mFpsAccumulator = 0;
                    mFpsCount = 0;
                }

                if (!isResizing)
                {
                    DrawBegin(); // virtual
                    Draw(gameTime); // virtual
                    DrawEnd(); // virtual
                }
            });

            UnloadContent(); // virtual
            Exiting(); // virtual
            Dispose();
        }

        /// <summary>
        /// Přenastaví parametry herního okna.
        /// </summary>
        /// <param name="window">Parametry herního okna, které se již aplikovali.</param>
        /// <remarks>Bázová funkce se volá až nakonec.</remarks>
        protected virtual void WindowConfiguration(ref WindowConfig window)
        {
            ApplyChanges();
        }

        /// <summary>
        /// Provede initializaci před spuštěním herního okna.
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// Vykresluje vše na okno.
        /// </summary>
        /// <param name="time">Herní čas.</param>
        protected virtual void Draw(GameTimeInfo time)
        {
        }

        /// <summary>
        /// Počátek vykreslení.
        /// </summary>
        protected virtual void DrawBegin()
        {
        }

        /// <summary>
        /// Konec vykreslení.
        /// </summary>
        protected virtual void DrawEnd()
        {
        }

        /// <summary>
        /// Změna velikosti herního okna.
        /// </summary>
        /// <param name="size">Nová velikost okna.</param>
        protected virtual void ResizeEnd(DrawingSize size)
        {
        }

        /// <summary>
        /// Aktualizuje objekty před jejich vykreslením.
        /// </summary>
        /// <param name="time">Herní čas.</param>
        protected virtual void Update(GameTimeInfo time)
        {
        }

        /// <summary>
        /// Načte potřebný obsah před spuštěním herního okna.
        /// </summary>
        protected virtual void LoadContent()
        {
        }

        /// <summary>
        /// Uvolní načtený obsah při zavření okna.
        /// </summary>
        protected virtual void UnloadContent()
        {
        }

        /// <summary>
        /// Herní okno je připraveno uvolnit prostředky a vypnout se.
        /// </summary>
        protected virtual void Exiting()
        {
        }
        
        /// <summary>
        /// Zachytí stisk klávesy.
        /// </summary>
        /// <param name="e">Informace o stisku klávesy.</param>
        protected virtual void KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Exit();
        }

        /// <summary>
        /// Zachytí uvolnění klávesy.
        /// </summary>
        /// <param name="e">Informace o uvolněné klávese.</param>
        protected virtual void KeyUp(KeyEventArgs e)
        {
        }

        /// <summary>
        /// Zachytí stisknutí tlačítka myši.
        /// </summary>
        /// <param name="e">Informace o stisknutém tlačítku.</param>
        protected virtual void MouseDown(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Zachytí uvolnění tlačítka myši.
        /// </summary>
        /// <param name="e">Informace o uvolněném tlačítku.</param>
        protected virtual void MouseUp(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Zachytí pohyb myši po herním okně.
        /// </summary>
        /// <param name="e">Informace o pohybu myši.</param>
        protected virtual void MouseMove(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Zachytí změnu hodnoty na kolečku myši.
        /// </summary>
        /// <param name="e">Informace o kolečku myši.</param>
        protected virtual void MouseWheel(MouseEventArgs e)
        {
        }
    }
}
