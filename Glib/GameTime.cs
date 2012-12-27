using System.Diagnostics;

namespace Glib
{
    /// <summary>
    /// Pracuje s herním časem.
    /// </summary>
    public sealed class GameTime
    {
        private Stopwatch _stopwatch = null;
        private double _lastUpdate = 0;

        /// <summary>
        /// Hlavní konstruktor.
        /// </summary>
        public GameTime()
        {
            _stopwatch = new Stopwatch();
        }

        /// <summary>
        /// Spustí stopky.
        /// </summary>
        public void Start()
        {
            _stopwatch.Start();
            _lastUpdate = 0;
        }

        /// <summary>
        /// Zastaví stopky.
        /// </summary>
        public void Stop()
        {
            _stopwatch.Stop();
        }

        /// <summary>
        /// Aktualizuje čas delta.
        /// </summary>
        /// <returns>Vrací čas delta.</returns>
        public double UpdateDeltaTime()
        {
            double now = ElapsedTime;
            double deltaTime = now - _lastUpdate;
            _lastUpdate = now;
            return deltaTime;
        }

        /// <summary>
        /// Uplynulý čas od startu stopek v sekundách.
        /// </summary>
        public double ElapsedTime
        {
            get { return _stopwatch.ElapsedMilliseconds * 0.001; }
        }

        /// <summary>
        /// Jsou stopky spuštěny?
        /// </summary>
        public bool IsRunning
        {
            get { return _stopwatch.IsRunning; }
        }
    }
}
