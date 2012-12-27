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
        private Factory mFactory = null;
        private FactoryWrite mFactoryWrite = null;
        private WindowRenderTarget mRenderTarget = null;

        /// <summary>
        /// Grafický ovladač.
        /// </summary>
        protected WindowRenderTarget Device
        {
            get { return mRenderTarget; }
        }

        /// <summary>
        /// Správce výroby objektů.
        /// </summary>
        protected FactoryWrite FactoryWrite
        {
            get { return mFactoryWrite; }
        }

        /// <summary>
        /// Provede initializaci před spuštěním herního okna.
        /// </summary>
        /// <param name="window">Parametry herního okna, které se již aplikovali.</param>
        /// <remarks>Je nutné poté zavolat funkci <c>ApplyChanges</c> a tím uložit změny.</remarks>
        protected override void Initialize(ref WindowParams window)
        {
            mFactory = new Factory();
            mFactoryWrite = new FactoryWrite();

            HwndRenderTargetProperties properties = new HwndRenderTargetProperties()
            {
                Hwnd = Handle,
                PixelSize = new DrawingSize(Width, Height),
                PresentOptions = WindowParams.VSync ? PresentOptions.None : PresentOptions.Immediately
            };

            mRenderTarget = new WindowRenderTarget(mFactory,
                new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)), properties);
            mRenderTarget.AntialiasMode = AntialiasMode.PerPrimitive;
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
            mRenderTarget.BeginDraw();
        }

        /// <summary>
        /// Konec vykreslení.
        /// </summary>
        protected override void DrawEnd()
        {
            base.DrawEnd();
            mRenderTarget.EndDraw();
        }
    }
}
