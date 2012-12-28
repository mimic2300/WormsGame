using SharpDX;
using SharpDX.DirectInput;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Windows;
using System;
using System.Diagnostics;

using Keyboard = Glib.Input.Keyboard;
using Mouse = Glib.Input.Mouse;

namespace Glib
{
    /// <summary>
    /// Herní okno.
    /// </summary>
    public abstract class GlibWindow : Game
    {
        /// <summary>
        /// Aktualizace okna.
        /// </summary>
        public event Action<GameTime> OnUpdate;

        private readonly Stopwatch fpsClock;
        private GraphicsDeviceManager gdm;
        private SpriteBatch sprite;
        private int fpsCount = 0;
        private float fps = 0;
        private Keyboard keyboard;
        private Mouse mouse;

        /// <summary>
        /// Hlavní konstruktor.
        /// </summary>
        public GlibWindow()
        {
            gdm = new GraphicsDeviceManager(this);
            fpsClock = new Stopwatch();

            keyboard = new Keyboard(this);
            mouse = new Mouse(this);

            IsFixedTimeStep = false;
            gdm.SynchronizeWithVerticalRetrace = false;

            Content.RootDirectory = "Content";

            VirtualConstructor(gdm);
        }

        /// <summary>
        /// Formulář okna.
        /// </summary>
        public RenderForm Form
        {
            get { return (RenderForm)Window.NativeWindow; }
        }

        /// <summary>
        /// Sprite pro kreslení.
        /// </summary>
        public SpriteBatch Sprite
        {
            get { return sprite; }
        }

        /// <summary>
        /// FPS.
        /// </summary>
        public float FPS
        {
            get { return fps; }
        }

        /// <summary>
        /// Informace o klávesnici.
        /// </summary>
        public Keyboard Keyboard
        {
            get { return keyboard; }
        }

        /// <summary>
        /// Informace o myši.
        /// </summary>
        public Mouse Mouse
        {
            get { return mouse; }
        }

        /// <summary>
        /// Šířka okna.
        /// </summary>
        public int Width
        {
            get { return Window.ClientBounds.Width; }
        }

        /// <summary>
        /// Výška okna.
        /// </summary>
        public int Height
        {
            get { return Window.ClientBounds.Height; }
        }

        /// <summary>
        /// Načte herní obsah.
        /// </summary>
        protected override void LoadContent()
        {
            sprite = new SpriteBatch(gdm.GraphicsDevice);

            base.LoadContent();
        }

        /// <summary>
        /// Před spuštěním herní smyčky.
        /// </summary>
        protected override void BeginRun()
        {
            fpsClock.Start();

            base.BeginRun();
        }

        /// <summary>
        /// Nastavení okna apod.
        /// </summary>
        protected override void Initialize()
        {
            Window.Title = "Glib Window";
            Form.Icon = null;

            base.Initialize();
        }

        /// <summary>
        /// Vykreslení na okno.
        /// </summary>
        /// <param name="time">Herní čas.</param>
        protected override void Draw(GameTime time)
        {
            Update(time);

            CalculateFps(time);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(time);
        }

        /// <summary>
        /// Aktualizace okna před vykreslením.
        /// </summary>
        /// <param name="time">Herní čas.</param>
        protected override void Update(GameTime time)
        {
            base.Update(time);

            OnUpdate(time);

            if (keyboard.State.IsPressed(Key.Escape))
                Exit();
        }

        /// <summary>
        /// Počítá FPS.
        /// </summary>
        /// <param name="time">Herni čas.</param>
        private void CalculateFps(GameTime time)
        {
            float totalSeconds = (float)time.TotalGameTime.TotalSeconds;

            fpsCount++;

            if (fpsClock.ElapsedMilliseconds > 1000.0f)
            {
                fps = fpsCount * 1000.0f / fpsClock.ElapsedMilliseconds;
                fpsCount = 0;
                fpsClock.Restart();
            }
        }

        /// <summary>
        /// Virtuální konstruktor.
        /// </summary>
        /// <param name="gdm">Manažer grafického ovladače.</param>
        protected virtual void VirtualConstructor(GraphicsDeviceManager gdm)
        {
        }
    }
}
