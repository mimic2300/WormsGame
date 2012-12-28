using System;

namespace WormsGame
{
    class Program
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
