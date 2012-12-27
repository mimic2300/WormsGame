namespace Glib
{
    /// <summary>
    /// Informace o herním času.
    /// </summary>
    public struct GameTimeInfo
    {
        /// <summary>
        /// Uplynulý čas od spuštění herního okna.
        /// </summary>
        public double ElapsedTime;

        /// <summary>
        /// Delta čas.
        /// </summary>
        public double DeltaTime;

        /// <summary>
        /// Hlavní konstruktor.
        /// </summary>
        /// <param name="elapsedTime">Uplynulý čas od spuštění herního okna.</param>
        /// <param name="deltaTime">Delta čas.</param>
        public GameTimeInfo(double elapsedTime, double deltaTime)
        {
            ElapsedTime = elapsedTime;
            DeltaTime = deltaTime;
        }
    }
}
