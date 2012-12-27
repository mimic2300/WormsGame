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
        private Direct3D _direct = null;
        private Device _device = null;

        /// <summary>
        /// Grafický ovladač.
        /// </summary>
        protected Device Device
        {
            get { return _device; }
        }

        /// <summary>
        /// Provede initializaci před spuštěním herního okna.
        /// </summary>
        /// <param name="window">Parametry herního okna, které se již aplikovali.</param>
        /// <remarks>Je nutné poté zavolat funkci <c>ApplyChanges</c> a tím uložit změny.</remarks>
        protected override void Initialize(ref WindowParams window)
        {
            _direct = new Direct3D();
            _device = new Device(_direct, 0, DeviceType.Hardware, WindowParams.Handle, CreateFlags.HardwareVertexProcessing,
                new PresentParameters(Width, Height));
        }

        /// <summary>
        /// Počátek vykreslení.
        /// </summary>
        protected override void DrawBegin()
        {
            _device.BeginScene();
        }

        /// <summary>
        /// Vykresluje vše na okno.
        /// </summary>
        /// <param name="time">Herní čas.</param>
        protected override void Draw(GameTimeInfo time)
        {
            _device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
        }

        /// <summary>
        /// Konec vykreslení.
        /// </summary>
        protected override void DrawEnd()
        {
            _device.EndScene();
            _device.Present();
        }
    }
}
