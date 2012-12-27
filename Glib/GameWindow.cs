using SharpDX.Windows;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Glib
{
    /// <summary>
    /// Základní objekt herního okna.
    /// </summary>
    public abstract class GameWindow : IDisposable
    {
        #region Proměnné

        private GameTime _gameTime = null;
        private RenderForm _form = null;
        // základní parametry herního okna, které lze měnit
        private WindowParams _windowParams;
        private double _deltaTime = 0;
        private int _fps = 0;
        // pouze počítadlo pro FPS
        private int _fpsCount = 0;
        // pouze sčítač delta času
        private double _fpsAccumulator = 0;

        #endregion Proměnné

        #region Konstruktory

        /// <summary>
        /// Hlavní konstruktor.
        /// </summary>
        public GameWindow()
        {
            _gameTime = new GameTime();

            _form = new RenderForm();
            _form.Icon = null;
            _form.Text = "Game Window";
            _form.FormBorderStyle = FormBorderStyle.FixedSingle;
            _form.MaximizeBox = false;
            _form.Size = new Size(720, 480);
            _form.StartPosition = FormStartPosition.CenterScreen;

            _form.KeyDown += new KeyEventHandler((o, e) => { KeyDown(e); });
            _form.KeyUp += new KeyEventHandler((o, e) => { KeyUp(e); });
            _form.MouseDown += new MouseEventHandler((o, e) => { MouseDown(e); });
            _form.MouseUp += new MouseEventHandler((o, e) => { MouseUp(e); });
            _form.MouseMove += new MouseEventHandler((o, e) => { MouseMove(e); });
            _form.MouseWheel += new MouseEventHandler((o, e) => { MouseWheel(e); });

            _windowParams = new WindowParams(_form);
        }

        /// <summary>
        /// Konstruktor pro nastavení titulku herního okna.
        /// </summary>
        /// <param name="title">Titulek okna.</param>
        public GameWindow(string title)
            : this()
        {
            _windowParams.Title = title;
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
            _windowParams.Width = width;
            _windowParams.Height = height;
        }

        #endregion Konstruktory

        #region Vlastnosti

        /// <summary>
        /// Parametry herního okna.
        /// </summary>
        public WindowParams WindowParams
        {
            get { return _windowParams; }
        }

        /// <summary>
        /// Získá nebo nastaví titulek okna.
        /// </summary>
        public string WindowTitle
        {
            get { return _windowParams.Title; }
            set
            {
                _windowParams.Title = value;
                _form.Text = value;
            }
        }

        /// <summary>
        /// Šířka herního okna.
        /// </summary>
        public int Width
        {
            get { return _windowParams.Width; }
        }

        /// <summary>
        /// Výška herního okna.
        /// </summary>
        public int Height
        {
            get { return _windowParams.Height; }
        }

        /// <summary>
        /// FPS.
        /// </summary>
        public int FPS
        {
            get { return _fps; }
        }

        /// <summary>
        /// Čas delta.
        /// </summary>
        public double DeltaTime
        {
            get { return _deltaTime; }
        }

        #endregion Vlastnosti

        /// <summary>
        /// Uvolní prostředky herního okna.
        /// </summary>
        public void Dispose()
        {
            if (_gameTime.IsRunning)
                _gameTime.Stop();

            _form.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Ukončí herní okno.
        /// </summary>
        public void Exit()
        {
            _form.Close();
        }

        /// <summary>
        /// Aplikuje změny, které se provedly v parametrech okna při initializaci.
        /// </summary>
        protected void ApplyChanges()
        {
            _form.Text = _windowParams.Title;
            _form.Icon = _windowParams.Icon;

            int width = Screen.PrimaryScreen.Bounds.Width - _windowParams.Width;
            int height = Screen.PrimaryScreen.Bounds.Height - _windowParams.Height;

            _form.Bounds = new Rectangle(width / 2, height / 2, _windowParams.Width, _windowParams.Height);
        }

        /// <summary>
        /// Spustí a zobrazí herní okno.
        /// </summary>
        public void Run()
        {
            Initialize(ref _windowParams); // abstract

            bool isResizing = false;
            bool isClosing = false;

            _form.ResizeBegin += new EventHandler((o, e) => { isResizing = true; });
            _form.ResizeEnd += new EventHandler((o, e) => { isResizing = false; });
            _form.FormClosing += new FormClosingEventHandler((o, e) => { isClosing = true; });

            LoadContent(); // virtual

            _gameTime.Start();

            RenderLoop.Run(_form, () =>
            {
                if (isClosing)
                    return;

                _deltaTime = _gameTime.UpdateDeltaTime();

                GameTimeInfo gameTime = new GameTimeInfo(_gameTime.ElapsedTime, _deltaTime);

                Update(gameTime); // virtual

                _fpsAccumulator += _deltaTime;
                ++_fpsCount;

                if (_fpsAccumulator >= 1.0)
                {
                    _fps = (int)(_fpsCount / _fpsAccumulator);
                    _fpsAccumulator = 0;
                    _fpsCount = 0;
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
        /// Provede initializaci před spuštěním herního okna.
        /// </summary>
        /// <param name="window">Parametry herního okna, které se již aplikovali.</param>
        /// <remarks>Je nutné poté zavolat funkci <c>ApplyChanges</c> a tím uložit změny.</remarks>
        protected abstract void Initialize(ref WindowParams window);

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
