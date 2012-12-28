using Glib;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

namespace WormsGame
{
    public sealed class WormsGameWindow : GameWindow2D
    {
        private System.Drawing.Point mMousePosition;
        private System.Windows.Forms.Keys mKeysDown;
        private int mMouseWheel = 0;
        private TextFormat mFont = null;
        private float mAngle = 0;

        protected override void WindowConfiguration(ref WindowConfig window)
        {
            window.Title = "Worms";
            window.VSync = false;

            base.WindowConfiguration(ref window);
        }

        protected override void Initialize()
        {
            base.Initialize();

            mFont = new TextFormat(FactoryWrite, "Consolas", 20f);

            Device.TextAntialiasMode = TextAntialiasMode.Aliased;

            Draw2D.Initialize(this, Device);
        }

        protected override void Update(GameTimeInfo time)
        {
            base.Update(time);

            mAngle += 60f * (float)time.DeltaTime;

            if (mAngle + 60f * (float)time.DeltaTime  > 360)
                mAngle = 0;
        }

        protected override void Draw(GameTimeInfo time)
        {
            base.Draw(time);

            Draw2D.Line(mMousePosition.X, 0, mMousePosition.X, Height, Color.Gray);
            Draw2D.Line(0, mMousePosition.Y, Width, mMousePosition.Y, Color.Gray);

            System.Console.WriteLine("Width: " + Width + ", Height: " + Height);

            Draw2D.Text("FPS: ", mFont, 10, 10, Color.White);
            Draw2D.Text("Delta time: ", mFont, 10, 30, Color.White);
            Draw2D.Text("Mouse position: ", mFont, 10, 50, Color.White);
            Draw2D.Text("Keys: ", mFont, 10, 70, Color.White);
            Draw2D.Text("Wheel: ", mFont, 10, 90, Color.White);
            Draw2D.Text("Angle: ", mFont, 10, 110, Color.White);

            Draw2D.Text(FPS, mFont, 60, 10, Color.LightGreen);
            Draw2D.Text(DeltaTime.ToString("F3"), mFont, 140, 30, Color.LightGreen);
            Draw2D.Text(mMousePosition.X + "," + mMousePosition.Y, mFont, 183, 50, Color.LightGreen);
            Draw2D.Text(mKeysDown, mFont, 80, 70, Color.LightGreen);
            Draw2D.Text(mMouseWheel, mFont, 80, 90, Color.LightGreen);
            Draw2D.Text(mAngle.ToString("F1") + "°", mFont, 80, 110, Color.LightGreen);

            Draw2D.Text("ěščřžýáíé - ďťňó - 0123456789 - hello world - !?&@~+-._,%#*><()[]{}", mFont, 10, Height - 30, Color.Yellow);
            Draw2D.Text("d(-_-)b", mFont, 600, 300, Color.Red, DrawTextOptions.None, MeasuringMode.Natural, mAngle, new Vector2(635, 315));

            Draw2D.Rectangle(300, 300, 100, 100, Color.DeepSkyBlue, 1, mAngle);
            Draw2D.FillRectangle(325, 325, 50, 50, Color.DarkOliveGreen, -mAngle);
            Draw2D.Line(300, 350, 400, 350, Color.Red, 1, null, mAngle);
            Draw2D.Line(350, 300, 350, 400, Color.Red, 1, null, mAngle);
            Draw2D.Ellipse(new DrawingPointF(150, 400), 50, 100, Color.Red, 1, null, mAngle);
            Draw2D.Ellipse(new DrawingPointF(150, 400), 50, 100, Color.Red, 1, null, -mAngle);
            Draw2D.Ellipse(new DrawingPointF(150, 400), 30, 30, Color.DarkSlateGray, 5);
        }

        protected override void MouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.MouseMove(e);

            mMousePosition = e.Location;
        }

        protected override void KeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            base.KeyDown(e);

            // urychlí rotaci
            if (e.KeyCode == System.Windows.Forms.Keys.Space)
                mAngle += 6f;

            mKeysDown = e.KeyCode;
        }

        protected override void KeyUp(System.Windows.Forms.KeyEventArgs e)
        {
            base.KeyUp(e);

            mKeysDown = System.Windows.Forms.Keys.None;
        }

        protected override void MouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            base.MouseWheel(e);

            mMouseWheel += e.Delta / 120;
        }
    }
}
