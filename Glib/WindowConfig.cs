using SharpDX.Windows;
using System;
using System.Drawing;

namespace Glib
{
    /// <summary>
    /// Parametry herního okna.
    /// </summary>
    public struct WindowConfig
    {
        #region Proměnné

        private IntPtr mHandle;

        /// <summary>
        /// Titulek okna.
        /// </summary>
        public string Title;

        /// <summary>
        /// Šířka okna.
        /// </summary>
        public int Width;

        /// <summary>
        /// Výška okna.
        /// </summary>
        public int Height;

        /// <summary>
        /// Ikonka okna.
        /// </summary>
        public Icon Icon;

        /// <summary>
        /// Má se použít vertikální synchronizace?
        /// </summary>
        public bool VSync;

        #endregion Proměnné

        /// <summary>
        /// Kopírovací konstruktor.
        /// </summary>
        /// <param name="window">Parametry herního okna.</param>
        public WindowConfig(WindowConfig window)
        {
            this = window;
        }

        /// <summary>
        /// Konstruktor, který získá parametry z herního okna.
        /// </summary>
        /// <param name="form"></param>
        public WindowConfig(RenderForm form)
        {
            mHandle = form.Handle;

            Title = form.Text;
            Width = form.Width;
            Height = form.Height;
            Icon = form.Icon;
            VSync = false;
        }

        /// <summary>
        /// Získá handle okna.
        /// </summary>
        public IntPtr Handle
        {
            get { return mHandle; }
        }
    }
}
