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

        protected override void Initialize()
        {
            base.Initialize();

            // nastavení okna
            Window.Title = "Worms";
            IsMouseVisible = true;

            // nastavení pohledu a projekce
            view = Matrix.LookAtLH(new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            projection = Matrix.PerspectiveFovLH(MathUtil.PiOverFour, 1, 1, 1000);

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

            Sprite.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied);

            Sprite.DrawString(font, "FPS: " + FPS.ToString("#"), new Vector2(10, 10), Color.Black);
            Sprite.DrawString(font, "Keyboard: " + Keyboard, new Vector2(10, 40), Color.Black);
            Sprite.DrawString(font, "Mouse: " + Mouse, new Vector2(10, 70), Color.Black);

            effect.VertexColorEnabled = true;
            effect.CurrentTechnique.Passes[0].Apply();

            lineVertex.Begin();
            lineVertex.DrawLine(
                new VertexPositionColor(new Vector3(0, 0, 0), Color.White),
                new VertexPositionColor(new Vector3(200, 200, 0), Color.White));
            lineVertex.End();

            Sprite.End();
        }
    }
}
