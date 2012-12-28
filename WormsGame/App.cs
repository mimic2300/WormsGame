using System;

namespace WormsGame
{
    internal class App
    {
#if NETFX_CORE
        [MTAThread]
#else
        [STAThread]
#endif
        static void Main()
        {
            using (WormsGameWindow window = new WormsGameWindow())
            {
                window.Run();
            }
        }
    }
}
