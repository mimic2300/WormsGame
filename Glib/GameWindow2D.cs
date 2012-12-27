using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;

using Factory = SharpDX.Direct2D1.Factory;
using FactoryWrite = SharpDX.DirectWrite.Factory;

namespace Glib
{
    /// <summary>
    /// Herní okno pro 2D grafiku (Direct2D1).
    /// </summary>
    public class GameWindow2D : GameWindow
    {
        private Factory _factory = null;
        private FactoryWrite _factoryWrite = null;
        private WindowRenderTarget _renderTarget = null;

        /// <summary>
        /// Grafický ovladač.
        /// </summary>
        protected WindowRenderTarget Device
        {
            get { return _renderTarget; }
        }

        /// <summary>
        /// Správce výroby objektů.
        /// </summary>
        protected FactoryWrite FactoryWrite
        {
            get { return _factoryWrite; }
        }

        /// <summary>
        /// Provede initializaci před spuštěním herního okna.
        /// </summary>
        /// <param name="window">Parametry herního okna, které se již aplikovali.</param>
        /// <remarks>Je nutné poté zavolat funkci <c>ApplyChanges</c> a tím uložit změny.</remarks>
        protected override void Initialize(ref WindowParams window)
        {
            _factory = new Factory();
            _factoryWrite = new FactoryWrite();

            HwndRenderTargetProperties properties = new HwndRenderTargetProperties()
            {
                Hwnd = WindowParams.Handle,
                PixelSize = new DrawingSize(Width, Height),
                PresentOptions = WindowParams.VSync ? PresentOptions.None : PresentOptions.Immediately
            };

            _renderTarget = new WindowRenderTarget(_factory,
                new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)), properties);
            _renderTarget.AntialiasMode = AntialiasMode.PerPrimitive;
        }

        /// <summary>
        /// Vykresluje vše na okno.
        /// </summary>
        /// <param name="time">Herní čas.</param>
        protected override void Draw(GameTimeInfo time)
        {
            Device.Clear(Color.Black);
        }

        /// <summary>
        /// Počátek vykreslení.
        /// </summary>
        protected override void DrawBegin()
        {
            base.DrawBegin();
            _renderTarget.BeginDraw();
        }

        /// <summary>
        /// Konec vykreslení.
        /// </summary>
        protected override void DrawEnd()
        {
            base.DrawEnd();
            _renderTarget.EndDraw();
        }
    }
}
