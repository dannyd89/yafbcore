using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Simples
{
    public static class Circle
    {
        /// <summary>
        /// Draws a non-filled circle
        /// </summary>
        /// <param name="renderTarget">Used to draw the circle on the target. Needs to have BeginDraw() called before</param>
        /// <param name="brush">Brush used to color the circle</param>
        /// <param name="center">Center of the circle</param>
        /// <param name="radius">Radius of the circle</param>
        /// <param name="strokeWidth">Stroke width of the circle line</param>
        public static void Draw(WindowRenderTarget renderTarget, Brush brush, Vector2 center, float radius, float strokeWidth = 1f, StrokeStyle strokeStyle = null)
        {
            if (renderTarget == null || brush == null || center == null)
                throw new ArgumentNullException("value is null");

            if (renderTarget.IsDisposed == true)
                throw new ArgumentNullException("Rendertarget is disposed");

            Ellipse ellipse = new Ellipse(center, radius, radius);

            if (strokeStyle == null)
                renderTarget.DrawEllipse(ellipse, brush, strokeWidth);
            else
                renderTarget.DrawEllipse(ellipse, brush, strokeWidth, strokeStyle);
        }

        /// <summary>
        /// Draws a filled circle
        /// </summary>
        /// <param name="renderTarget">Used to draw the circle on the target. Needs to have BeginDraw() called before</param>
        /// <param name="brush">Brush used to color the circle</param>
        /// <param name="center">Center of the circle</param>
        /// <param name="radius">Radius of the circle</param>
        public static void Fill(WindowRenderTarget renderTarget, Brush brush, Vector2 center, float radius)
        {
            if (renderTarget == null || brush == null || center == null)
                throw new ArgumentNullException("value is null");

            if (renderTarget.IsDisposed == true)
                throw new ArgumentNullException("Rendertarget is disposed");

            Ellipse ellipse = new Ellipse(center, radius, radius);

            renderTarget.FillEllipse(ellipse, brush);
        }
    }
}
