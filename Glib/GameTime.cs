using System.Diagnostics;

namespace Glib
{
    /// <summary>
    /// Pracuje s herním časem.
    /// </summary>
    public sealed class GameTime
    {
        private Stopwatch mStopwatch = null;
        private double mLastUpdate = 0;

        /// <summary>
        /// Hlavní konstruktor.
        /// </summary>
        public GameTime()
        {
            mStopwatch = new Stopwatch();
        }

        /// <summary>
        /// Spustí stopky.
        /// </summary>
        public void Start()
        {
            mStopwatch.Start();
            mLastUpdate = 0;
        }

        /// <summary>
        /// Zastaví stopky.
        /// </summary>
        public void Stop()
        {
            mStopwatch.Stop();
        }

        /// <summary>
        /// Aktualizuje čas delta.
        /// </summary>
        /// <returns>Vrací čas delta.</returns>
        public double UpdateDeltaTime()
        {
            double now = ElapsedTime;
            double deltaTime = now - mLastUpdate;
            mLastUpdate = now;
            return deltaTime;
        }

        /// <summary>
        /// Uplynulý čas od startu stopek v sekundách.
        /// </summary>
        public double ElapsedTime
        {
            get { return mStopwatch.ElapsedMilliseconds * 0.001; }
        }

        /// <summary>
        /// Jsou stopky spuštěny?
        /// </summary>
        public bool IsRunning
        {
            get { return mStopwatch.IsRunning; }
        }
    }
}
