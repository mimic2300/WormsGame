using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System.Diagnostics;

namespace Glib
{
    /// <summary>
    /// Herní okno.
    /// </summary>
    public abstract class GlibWindow : Game
    {
        private readonly Stopwatch fpsClock;
        private GraphicsDeviceManager gdm;
        private SpriteBatch sprite;
        private int fpsCount = 0;
        private float fps = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentDirectory"></param>
        /// <param name="vsync"></param>
        public GlibWindow(string contentDirectory = "Content", bool vsync = false)
        {
            gdm = new GraphicsDeviceManager(this);
            fpsClock = new Stopwatch();

            // nastavuje vertikální synchronizaci
            gdm.SynchronizeWithVerticalRetrace = vsync;
            IsFixedTimeStep = vsync;

            // nastavuje adresář pro herní obsah
            Content.RootDirectory = contentDirectory;
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

            base.Initialize();
        }

        /// <summary>
        /// Vykreslení na okno.
        /// </summary>
        /// <param name="time">Herní čas.</param>
        protected override void Draw(GameTime time)
        {
            Update(time);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            float totalSeconds = (float)time.TotalGameTime.TotalSeconds;

            base.Draw(time);

            fpsCount++;

            if (fpsClock.ElapsedMilliseconds > 1000.0f)
            {
                fps = fpsCount * 1000.0f / fpsClock.ElapsedMilliseconds;
                fpsCount = 0;
                fpsClock.Restart();
            }
        }

        /// <summary>
        /// Aktualizace před vykreslením.
        /// </summary>
        /// <param name="time">Herní čas.</param>
        protected virtual void Update(GameTime time)
        {
        }
    }
}
