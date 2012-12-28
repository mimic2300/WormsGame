using Glib;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace WormsGame
{
    internal sealed class WormsGameWindow : GlibWindow
    {
        private SpriteFont font;

        protected override void Initialize()
        {
            base.Initialize();

            Window.Title = "Worms";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            font = Content.Load<SpriteFont>("consolas.ft");
        }

        protected override void Draw(GameTime time)
        {
            base.Draw(time);

            Sprite.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied);

            Sprite.DrawString(font, "FPS: " + FPS, new Vector2(10, 10), Color.Black);

            Sprite.End();
        }
    }
}
