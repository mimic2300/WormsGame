using System;

namespace WormsGame
{
    static class App
    {
        [STAThread]
        static void Main()
        {
            new WormsGameWindow().Run();
        }
    }
}
