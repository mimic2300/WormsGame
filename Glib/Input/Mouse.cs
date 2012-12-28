using SharpDX.DirectInput;

using MouseDirect = SharpDX.DirectInput.Mouse;

namespace Glib.Input
{
    /// <summary>
    /// Získá informace o myši.
    /// </summary>
    public class Mouse : InputBase
    {
        private MouseDirect mouse;
        private MouseState state;

        /// <summary>
        /// Hlavní konstruktor.
        /// </summary>
        /// <param name="window">Herní okno.</param>
        public Mouse(GlibWindow window)
            : base(window)
        {
            mouse = new MouseDirect(Input);
            mouse.Properties.BufferSize = 128;
            mouse.Acquire();

            state = new MouseState();
        }

        /// <summary>
        /// Stav myši.
        /// </summary>
        public MouseState State
        {
            get { return state; }
        }

        /// <summary>
        /// Volá se stejně jako aktualizace okna.
        /// </summary>
        protected override void Update()
        {
            //mouse.GetCurrentState(ref state);

            var raw = mouse.GetBufferedData();

            foreach (var s in raw)
                System.Console.WriteLine(s);
        }

        /// <summary>
        /// Získá informace o myši.
        /// </summary>
        /// <returns>Informace o myši.</returns>
        public override string ToString()
        {
            return string.Format("[{0},{1},{2}] {3}", state.X, state.Y, state.Z, state.Buttons);
        }
    }
}
