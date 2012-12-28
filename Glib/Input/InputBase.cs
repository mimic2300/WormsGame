using SharpDX.DirectInput;

namespace Glib.Input
{
    /// <summary>
    /// Získá vstup z okna přes DirectInput.
    /// </summary>
    public abstract class InputBase
    {
        private DirectInput input;

        /// <summary>
        /// Hlavní konstruktor.
        /// </summary>
        /// <param name="window">Herní okno.</param>
        public InputBase(GlibWindow window)
        {
            input = new DirectInput();

            window.OnUpdate += Update;
        }

        /// <summary>
        /// DirectInput.
        /// </summary>
        protected DirectInput Input
        {
            get { return input; }
        }

        /// <summary>
        /// Volá se stejně jako aktualizace okna.
        /// </summary>
        protected abstract void Update();
    }
}
