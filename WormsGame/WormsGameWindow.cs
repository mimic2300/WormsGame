using Glib;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

namespace WormsGame
{
    public sealed class WormsGameWindow : GameWindow2D
    {
        private System.Drawing.Point _mousePosition;
        private System.Windows.Forms.Keys _keysDown;
        private int _mouseWheel = 0;

        private TextFormat _font = null;
        private SolidColorBrush _brush = null;

        protected override void Initialize(ref WindowParams window)
        {
            window.Title = "Worms";
            window.VSync = false;
            ApplyChanges();

            base.Initialize(ref window);

            _font = new TextFormat(FactoryWrite, "Consolas", 20f);
            _brush = new SolidColorBrush(Device, Color.White);

            Device.TextAntialiasMode = TextAntialiasMode.Aliased;
        }

        protected override void Draw(GameTimeInfo time)
        {
            base.Draw(time);

            _brush.Color = Color.Gray;
            Device.DrawLine(new DrawingPointF(_mousePosition.X, 0), new DrawingPointF(_mousePosition.X, Height), _brush);
            Device.DrawLine(new DrawingPointF(0, _mousePosition.Y), new DrawingPointF(Width, _mousePosition.Y), _brush);

            _brush.Color = Color.White;
            Device.DrawText("FPS: ", _font, new RectangleF(10, 10, Width, Height), _brush);
            Device.DrawText("Delta time: ", _font, new RectangleF(10, 30, Width, Height), _brush);
            Device.DrawText("Mouse position: ", _font, new RectangleF(10, 50, Width, Height), _brush);
            Device.DrawText("Keys: ", _font, new RectangleF(10, 70, Width, Height), _brush);
            Device.DrawText("Wheel: ", _font, new RectangleF(10, 90, Width, Height), _brush);

            _brush.Color = Color.LightGreen;
            Device.DrawText(FPS.ToString(), _font, new RectangleF(60, 10, Width, Height), _brush);
            Device.DrawText(DeltaTime.ToString("F3"), _font, new RectangleF(140, 30, Width, Height), _brush);
            Device.DrawText("x=" + _mousePosition.X + ", y=" + _mousePosition.Y, _font, new RectangleF(183, 50, Width, Height), _brush);
            Device.DrawText(_keysDown.ToString(), _font, new RectangleF(80, 70, Width, Height), _brush);
            Device.DrawText(_mouseWheel.ToString(), _font, new RectangleF(80, 90, Width, Height), _brush);
        }

        protected override void MouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.MouseMove(e);

            _mousePosition = e.Location;
        }

        protected override void KeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            base.KeyDown(e);

            _keysDown = e.KeyCode;
        }

        protected override void KeyUp(System.Windows.Forms.KeyEventArgs e)
        {
            base.KeyUp(e);

            _keysDown = System.Windows.Forms.Keys.None;
        }

        protected override void MouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            base.MouseWheel(e);

            _mouseWheel += e.Delta / 120;
        }
    }
}
