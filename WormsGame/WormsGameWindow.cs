using Glib;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace WormsGame
{
    internal sealed class WormsGameWindow : GlibWindow
    {
        private SpriteFont font;
        private BasicEffect effect;
        private Matrix view;
        private Matrix projection;
        private PrimitiveBatch<VertexPositionColor> lineVertex;

        protected override void VirtualConstructor(GraphicsDeviceManager gdm)
        {
            gdm.PreferMultiSampling = true;
            gdm.PreferredBackBufferWidth = 1280;
            gdm.PreferredBackBufferHeight = 800;
            //gdm.SynchronizeWithVerticalRetrace = true;
            //IsFixedTimeStep = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            IsMouseVisible = true;
            Window.Title = "Worms";
            Window.AllowUserResizing = true;

            // nastavení pohledu a projekce
            view = Matrix.LookAtLH(new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            projection = Matrix.OrthoLH(Width, Height, 1, 0);

            // vytvoření a nastavení effektu (aplikování pohledu a projekce)
            effect = new BasicEffect(GraphicsDevice);
            effect.World = Matrix.Identity;
            effect.View = view;
            effect.Projection = projection;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            font = Content.Load<SpriteFont>("consolas.ft");
            lineVertex = new PrimitiveBatch<VertexPositionColor>(GraphicsDevice);
        }

        protected override void Draw(GameTime time)
        {
            base.Draw(time);

            GraphicsDevice.Clear(new Color(32, 32, 32, 0));

            Sprite.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied);

            Sprite.DrawString(font, "FPS: " + FPS.ToString("#"), new Vector2(10, 10), Color.Lime);
            Sprite.DrawString(font, "Keyboard: " + Keyboard, new Vector2(10, 40), Color.Gray);
            Sprite.DrawString(font, "Mouse: " + Mouse, new Vector2(10, 70), Color.Gray);
            Sprite.DrawString(font, "Window: " + Width + "x" + Height, new Vector2(10, 100), Color.Gray);

            effect.VertexColorEnabled = true;
            effect.CurrentTechnique.Passes[0].Apply();

            lineVertex.Begin();
            lineVertex.DrawLine(
                new VertexPositionColor(new Vector3(50, 0, 0), Color.White),
                new VertexPositionColor(new Vector3(-50, 0, 0), Color.White));
            lineVertex.DrawLine(
                new VertexPositionColor(new Vector3(0, 50, 0), Color.Red),
                new VertexPositionColor(new Vector3(0, -50, 0), Color.Red));
            lineVertex.End();

            Sprite.End();
        }
    }
}
