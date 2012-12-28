using SharpDX.DirectInput;

using KeyboardDirect = SharpDX.DirectInput.Keyboard;

namespace Glib.Input
{
    /// <summary>
    /// Získá informace o klávesnici.
    /// </summary>
    public class Keyboard : InputBase
    {
        private KeyboardDirect keyboard;
        private KeyboardState state;

        /// <summary>
        /// Hlavní konstruktor.
        /// </summary>
        /// <param name="window">Herní okno.</param>
        public Keyboard(GlibWindow window)
            : base(window)
        {
            keyboard = new KeyboardDirect(Input);
            keyboard.Properties.BufferSize = 128;
            keyboard.Acquire();

            state = new KeyboardState();
        }

        /// <summary>
        /// Stav klávesnice.
        /// </summary>
        public KeyboardState State
        {
            get { return state; }
        }

        /// <summary>
        /// Volá se stejně jako aktualizace okna.
        /// </summary>
        protected override void Update()
        {
            keyboard.GetCurrentState(ref state);
        }

        /// <summary>
        /// Získá seznam stisknutých kláves.
        /// </summary>
        /// <returns>Seznam stisknutých kláves.</returns>
        public override string ToString()
        {
            string output = null;

            for (int i = 0; i < state.PressedKeys.Count; i++)
            {
                output += state.PressedKeys[i].ToString();

                if (i < state.PressedKeys.Count - 1)
                    output += " + ";
            }
            return (output == null) ? "None" : output;
        }
    }
}
