using SharpDX;
using SharpDX.Direct3D9;

using Device = SharpDX.Direct3D9.Device;

namespace Glib
{
    /// <summary>
    /// Herní okno přes DirectX 9.0c.
    /// </summary>
    public class GameWindowDx9 : GameWindow
    {
        private Direct3D mDirect = null;
        private Device mDevice = null;

        /// <summary>
        /// Grafický ovladač.
        /// </summary>
        protected Device Device
        {
            get { return mDevice; }
        }

        /// <summary>
        /// Provede initializaci před spuštěním herního okna.
        /// </summary>
        /// <param name="window">Parametry herního okna, které se již aplikovali.</param>
        /// <remarks>Je nutné poté zavolat funkci <c>ApplyChanges</c> a tím uložit změny.</remarks>
        protected override void Initialize(ref WindowParams window)
        {
            mDirect = new Direct3D();
            mDevice = new Device(mDirect, 0, DeviceType.Hardware, Handle, CreateFlags.HardwareVertexProcessing,
                new PresentParameters(Width, Height));
        }

        /// <summary>
        /// Počátek vykreslení.
        /// </summary>
        protected override void DrawBegin()
        {
            mDevice.BeginScene();
        }

        /// <summary>
        /// Vykresluje vše na okno.
        /// </summary>
        /// <param name="time">Herní čas.</param>
        protected override void Draw(GameTimeInfo time)
        {
            mDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
        }

        /// <summary>
        /// Konec vykreslení.
        /// </summary>
        protected override void DrawEnd()
        {
            mDevice.EndScene();
            mDevice.Present();
        }
    }
}
