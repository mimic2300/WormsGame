using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

using Matrix = SharpDX.Matrix;

namespace Glib
{
    /// <summary>
    /// Vykresluje 2D grafiku.
    /// </summary>
    public static class Draw2D
    {
        private static GameWindow mWindow = null;
        private static SolidColorBrush mSolidBrush = null;
        private static WindowRenderTarget mDevice = null;

        /// <summary>
        /// Initializuje všechny potřebné prvky pro kreslení.
        /// </summary>
        /// <param name="window">Herní okno.</param>
        /// <param name="device">Grafický ovladač pro kreslení.</param>
        public static void Initialize(GameWindow2D window, WindowRenderTarget device)
        {
            if (window != null && device != null)
            {
                mWindow = window;
                mDevice = device;

                if (mSolidBrush == null)
                    mSolidBrush = new SolidColorBrush(device, Color.White);
            }
        }

        /// <summary>
        /// Grafický ovladač pro kreslení.
        /// </summary>
        public static WindowRenderTarget Device
        {
            get { return mDevice; }
        }

        /// <summary>
        /// Plný štětec pro kreslení.
        /// </summary>
        public static SolidColorBrush SolidBrush
        {
            get { return mSolidBrush; }
        }

        #region Text

        /// <summary>
        /// Vykreslí text.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="font">Font textu.</param>
        /// <param name="x">Pozice X.</param>
        /// <param name="y">Pozice Y.</param>
        /// <param name="color">Barva.</param>
        /// <param name="options">Nastavení textu.</param>
        /// <param name="mode">Typ míry textu.</param>
        /// <param name="angle">Úhel natočení.</param>
        /// <param name="origin">Střed natočení.</param>
        public static void Text(object text, TextFormat font, float x, float y, Color color, DrawTextOptions options, MeasuringMode mode, float angle, Vector2 origin)
        {
            mSolidBrush.Color = color;

            Matrix3x2 tempTransform = mDevice.Transform;

            mDevice.Transform = Matrix.Transformation2D(
                new Vector2(0, 0),
                0,
                new Vector2(1, 1),
                origin,
                MathUtil.DegreesToRadians(angle),
                new Vector2(0, 0));
            mDevice.DrawText(text.ToString(), font, new RectangleF(x, y, mWindow.Width, mWindow.Height), mSolidBrush, options, mode);
            mDevice.Transform = tempTransform;
        }

        /// <summary>
        /// Vykreslí text.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="font">Font textu.</param>
        /// <param name="x">Pozice X.</param>
        /// <param name="y">Pozice Y.</param>
        /// <param name="color">Barva.</param>
        /// <param name="options">Nastavení textu.</param>
        /// <param name="mode">Typ míry textu.</param>
        /// <param name="angle">Úhel natočení.</param>
        public static void Text(object text, TextFormat font, float x, float y, Color color, DrawTextOptions options, MeasuringMode mode, float angle)
        {
            Text(text, font, x, y, color, options, mode, angle, new Vector2(mWindow.Width / 2, mWindow.Height / 2));
        }

        /// <summary>
        /// Vykreslí text.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="font">Font textu.</param>
        /// <param name="x">Pozice X.</param>
        /// <param name="y">Pozice Y.</param>
        /// <param name="color">Barva.</param>
        /// <param name="options">Nastavení textu.</param>
        /// <param name="mode">Typ míry textu.</param>
        public static void Text(object text, TextFormat font, float x, float y, Color color, DrawTextOptions options, MeasuringMode mode)
        {
            mSolidBrush.Color = color;
            mDevice.DrawText(text.ToString(), font, new RectangleF(x, y, mWindow.Width, mWindow.Height), mSolidBrush, options, mode);
        }

        /// <summary>
        /// Vykreslí text.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="font">Font textu.</param>
        /// <param name="x">Pozice X.</param>
        /// <param name="y">Pozice Y.</param>
        /// <param name="color">Barva.</param>
        /// <param name="options">Nastavení textu.</param>
        public static void Text(object text, TextFormat font, float x, float y, Color color, DrawTextOptions options)
        {
            Text(text, font, x, y, color, DrawTextOptions.None, MeasuringMode.Natural);
        }

        /// <summary>
        /// Vykreslí text.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="font">Font textu.</param>
        /// <param name="x">Pozice X.</param>
        /// <param name="y">Pozice Y.</param>
        /// <param name="color">Barva.</param>
        public static void Text(object text, TextFormat font, float x, float y, Color color)
        {
            Text(text, font, x, y, color, DrawTextOptions.None);
        }

        #endregion Text

        #region Line

        /// <summary>
        /// Vykreslí čáru.
        /// </summary>
        /// <param name="x1">Počáteční pozice X.</param>
        /// <param name="y1">Počáteční pozice Y.</param>
        /// <param name="x2">Konečná pozice X.</param>
        /// <param name="y2">Konečná pozice Y.</param>
        /// <param name="color">Barva.</param>
        /// <param name="strokeWidth">Šířka čáry.</param>
        /// <param name="strokeStyle">Styl čáry.</param>
        /// <param name="angle">Úhel natočení.</param>
        /// <param name="origin">Střed natočení.</param>
        public static void Line(float x1, float y1, float x2, float y2, Color color, float strokeWidth, StrokeStyle strokeStyle, float angle, Vector2 origin)
        {
            mSolidBrush.Color = color;

            Matrix3x2 tempTransform = mDevice.Transform;

            mDevice.Transform = Matrix.Transformation2D(
                new Vector2(0, 0),
                0,
                new Vector2(1, 1),
                origin,
                MathUtil.DegreesToRadians(angle),
                new Vector2(0, 0));
            mDevice.DrawLine(new DrawingPointF(x1, y1), new DrawingPointF(x2, y2), SolidBrush, strokeWidth, strokeStyle);
            mDevice.Transform = tempTransform;
        }

        /// <summary>
        /// Vykreslí čáru.
        /// </summary>
        /// <param name="x1">Počáteční pozice X.</param>
        /// <param name="y1">Počáteční pozice Y.</param>
        /// <param name="x2">Konečná pozice X.</param>
        /// <param name="y2">Konečná pozice Y.</param>
        /// <param name="color">Barva.</param>
        /// <param name="strokeWidth">Šířka čáry.</param>
        /// <param name="strokeStyle">Styl čáry.</param>
        /// <param name="angle">Úhel natočení.</param>
        public static void Line(float x1, float y1, float x2, float y2, Color color, float strokeWidth, StrokeStyle strokeStyle, float angle)
        {
            Line(x1, y1, x2, y2, color, strokeWidth, strokeStyle, angle, new Vector2((x1 + x2) / 2, (y1 + y2) / 2));
        }

        /// <summary>
        /// Vykreslí čáru.
        /// </summary>
        /// <param name="x1">Počáteční pozice X.</param>
        /// <param name="y1">Počáteční pozice Y.</param>
        /// <param name="x2">Konečná pozice X.</param>
        /// <param name="y2">Konečná pozice Y.</param>
        /// <param name="color">Barva.</param>
        /// <param name="strokeWidth">Šířka čáry.</param>
        /// <param name="strokeStyle">Styl čáry.</param>
        public static void Line(float x1, float y1, float x2, float y2, Color color, float strokeWidth, StrokeStyle strokeStyle)
        {
            mSolidBrush.Color = color;
            mDevice.DrawLine(new DrawingPointF(x1, y1), new DrawingPointF(x2, y2), SolidBrush, strokeWidth, strokeStyle);
        }

        /// <summary>
        /// Vykreslí čáru.
        /// </summary>
        /// <param name="x1">Počáteční pozice X.</param>
        /// <param name="y1">Počáteční pozice Y.</param>
        /// <param name="x2">Konečná pozice X.</param>
        /// <param name="y2">Konečná pozice Y.</param>
        /// <param name="color">Barva.</param>
        /// <param name="strokeWidth">Šířka čáry.</param>
        public static void Line(float x1, float y1, float x2, float y2, Color color, float strokeWidth)
        {
            Line(x1, y1, x2, y2, color, strokeWidth, null);
        }

        /// <summary>
        /// Vykreslí čáru.
        /// </summary>
        /// <param name="x1">Počáteční pozice X.</param>
        /// <param name="y1">Počáteční pozice Y.</param>
        /// <param name="x2">Konečná pozice X.</param>
        /// <param name="y2">Konečná pozice Y.</param>
        /// <param name="color">Barva.</param>
        public static void Line(float x1, float y1, float x2, float y2, Color color)
        {
            Line(x1, y1, x2, y2, color, 1);
        }

        #endregion Line

        #region Rectangle

        /// <summary>
        /// Vykreslení obdelník.
        /// </summary>
        /// <param name="x">Pozice X.</param>
        /// <param name="y">Pozice Y.</param>
        /// <param name="width">Šířka.</param>
        /// <param name="height">Výška.</param>
        /// <param name="color">Barva.</param>
        /// <param name="strokeWidth">Šířka čar.</param>
        /// <param name="angle">Úhel natočení.</param>
        /// <param name="origin">Střed natočení.</param>
        public static void Rectangle(float x, float y, float width, float height, Color color, float strokeWidth, float angle, Vector2 origin)
        {
            mSolidBrush.Color = color;

            Matrix3x2 tempTransform = mDevice.Transform;

            mDevice.Transform = Matrix.Transformation2D(
                new Vector2(0, 0),
                0,
                new Vector2(1, 1),
                origin,
                MathUtil.DegreesToRadians(angle),
                new Vector2(0, 0));
            mDevice.DrawRectangle(new RectangleF(x, y, x + width, y + height), mSolidBrush, strokeWidth);
            mDevice.Transform = tempTransform;
        }

        /// <summary>
        /// Vykreslení obdelník.
        /// </summary>
        /// <param name="x">Pozice X.</param>
        /// <param name="y">Pozice Y.</param>
        /// <param name="width">Šířka.</param>
        /// <param name="height">Výška.</param>
        /// <param name="color">Barva.</param>
        /// <param name="strokeWidth">Šířka čar.</param>
        /// <param name="angle">Úhel natočení.</param>
        public static void Rectangle(float x, float y, float width, float height, Color color, float strokeWidth, float angle)
        {
            Rectangle(x, y, width, height, color, strokeWidth, angle, new Vector2(x + width / 2, y + height / 2));
        }

        /// <summary>
        /// Vykreslení obdelník.
        /// </summary>
        /// <param name="x">Pozice X.</param>
        /// <param name="y">Pozice Y.</param>
        /// <param name="width">Šířka.</param>
        /// <param name="height">Výška.</param>
        /// <param name="color">Barva.</param>
        /// <param name="strokeWidth">Šířka čar.</param>
        public static void Rectangle(float x, float y, float width, float height, Color color, float strokeWidth)
        {
            mSolidBrush.Color = color;
            mDevice.DrawRectangle(new RectangleF(x, y, x + width, y + height), mSolidBrush, strokeWidth);
        }

        /// <summary>
        /// Vykreslení obdelník.
        /// </summary>
        /// <param name="x">Pozice X.</param>
        /// <param name="y">Pozice Y.</param>
        /// <param name="width">Šířka.</param>
        /// <param name="height">Výška.</param>
        /// <param name="color">Barva.</param>
        public static void Rectangle(float x, float y, float width, float height, Color color)
        {
            Rectangle(x, y, width, height, color, 1);
        }

        #endregion Rectangle

        #region FillRectangle

        /// <summary>
        /// Vykreslení plný obdelník.
        /// </summary>
        /// <param name="x">Pozice X.</param>
        /// <param name="y">Pozice Y.</param>
        /// <param name="width">Šířka.</param>
        /// <param name="height">Výška.</param>
        /// <param name="color">Barva.</param>
        /// <param name="angle">Úhel natočení.</param>
        /// <param name="origin">Střed natočení.</param>
        public static void FillRectangle(float x, float y, float width, float height, Color color, float angle, Vector2 origin)
        {
            mSolidBrush.Color = color;

            Matrix3x2 tempTransform = mDevice.Transform;

            mDevice.Transform = Matrix.Transformation2D(
                new Vector2(0, 0),
                0,
                new Vector2(1, 1),
                origin,
                MathUtil.DegreesToRadians(angle),
                new Vector2(0, 0));
            mDevice.FillRectangle(new RectangleF(x, y, x + width, y + height), mSolidBrush);
            mDevice.Transform = tempTransform;
        }

        /// <summary>
        /// Vykreslení plný obdelník.
        /// </summary>
        /// <param name="x">Pozice X.</param>
        /// <param name="y">Pozice Y.</param>
        /// <param name="width">Šířka.</param>
        /// <param name="height">Výška.</param>
        /// <param name="color">Barva.</param>
        /// <param name="angle">Úhel natočení.</param>
        public static void FillRectangle(float x, float y, float width, float height, Color color, float angle)
        {
            FillRectangle(x, y, width, height, color, angle, new Vector2(x + width / 2, y + height / 2));
        }

        /// <summary>
        /// Vykreslení plný obdelník.
        /// </summary>
        /// <param name="x">Pozice X.</param>
        /// <param name="y">Pozice Y.</param>
        /// <param name="width">Šířka.</param>
        /// <param name="height">Výška.</param>
        /// <param name="color">Barva.</param>
        public static void FillRectangle(float x, float y, float width, float height, Color color)
        {
            mSolidBrush.Color = color;
            mDevice.FillRectangle(new RectangleF(x, y, x + width, y + height), mSolidBrush);
        }

        #endregion FillRectangle

        #region Ellipse

        /// <summary>
        /// Vykreslí elipsu.
        /// </summary>
        /// <param name="center">Střed elipsy.</param>
        /// <param name="width">Šířka.</param>
        /// <param name="height">Výška.</param>
        /// <param name="color">Barva.</param>
        /// <param name="strokeWidth">Šířka čáry.</param>
        /// <param name="strokeStyle">Styl čáry.</param>
        /// <param name="angle">Úhel natočení.</param>
        /// <param name="origin">Střed natočení.</param>
        public static void Ellipse(DrawingPointF center, float width, float height, Color color, float strokeWidth, StrokeStyle strokeStyle, float angle, Vector2 origin)
        {
            mSolidBrush.Color = color;

            Matrix3x2 tempTransform = mDevice.Transform;

            mDevice.Transform = Matrix.Transformation2D(
                new Vector2(0, 0),
                0,
                new Vector2(1, 1),
                origin,
                MathUtil.DegreesToRadians(angle),
                new Vector2(0, 0));
            mDevice.DrawEllipse(new Ellipse(center, width, height), mSolidBrush, strokeWidth, strokeStyle);
            mDevice.Transform = tempTransform;
        }

        /// <summary>
        /// Vykreslí elipsu.
        /// </summary>
        /// <param name="center">Střed elipsy.</param>
        /// <param name="width">Šířka.</param>
        /// <param name="height">Výška.</param>
        /// <param name="color">Barva.</param>
        /// <param name="strokeWidth">Šířka čáry.</param>
        /// <param name="strokeStyle">Styl čáry.</param>
        /// <param name="angle">Úhel natočení.</param>
        public static void Ellipse(DrawingPointF center, float width, float height, Color color, float strokeWidth, StrokeStyle strokeStyle, float angle)
        {
            Ellipse(center, width, height, color, strokeWidth, strokeStyle, angle, new Vector2(center.X, center.Y));
        }

        /// <summary>
        /// Vykreslí elipsu.
        /// </summary>
        /// <param name="center">Střed elipsy.</param>
        /// <param name="width">Šířka.</param>
        /// <param name="height">Výška.</param>
        /// <param name="color">Barva.</param>
        /// <param name="strokeWidth">Šířka čáry.</param>
        /// <param name="strokeStyle">Styl čáry.</param>
        public static void Ellipse(DrawingPointF center, float width, float height, Color color, float strokeWidth, StrokeStyle strokeStyle)
        {
            mSolidBrush.Color = color;
            mDevice.DrawEllipse(new Ellipse(center, width, height), mSolidBrush, strokeWidth, strokeStyle);
        }

        /// <summary>
        /// Vykreslí elipsu.
        /// </summary>
        /// <param name="center">Střed elipsy.</param>
        /// <param name="width">Šířka.</param>
        /// <param name="height">Výška.</param>
        /// <param name="color">Barva.</param>
        /// <param name="strokeWidth">Šířka čáry.</param>
        public static void Ellipse(DrawingPointF center, float width, float height, Color color, float strokeWidth)
        {
            Ellipse(center, width, height, color, strokeWidth, null);
        }

        /// <summary>
        /// Vykreslí elipsu.
        /// </summary>
        /// <param name="center">Střed elipsy.</param>
        /// <param name="width">Šířka.</param>
        /// <param name="height">Výška.</param>
        /// <param name="color">Barva.</param>
        public static void Ellipse(DrawingPointF center, float width, float height, Color color)
        {
            Ellipse(center, width, height, color, 1);
        }

        #endregion Ellipse
    }
}
