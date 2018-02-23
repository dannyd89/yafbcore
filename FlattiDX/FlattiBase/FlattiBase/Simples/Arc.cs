using SharpDX;
using SharpDX.Direct2D1;
using System;

namespace FlattiBase.Simples
{
    /// <summary>
    /// Draws an arc
    /// </summary>
    public static class Arc
    {
        public static SharpDX.Direct2D1.Factory Factory;

        /// <summary>
        /// Draws an arc with the passed parameters
        /// Take care with the usage, because arcs are costly to draw
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="renderTarget"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="startAngle"></param>
        /// <param name="sweepAngle"></param>
        /// <param name="brush"></param>
        /// <param name="strokeWidth"></param>
        public static void Draw(SharpDX.Direct2D1.RenderTarget renderTarget,
                                Brush brush,
                                SharpDX.Vector2 center,
                                float radius, 
                                float startAngle, 
                                float sweepAngle, 
                                float strokeWidth = 1f)
        {
            if (sweepAngle <= 0f)
                throw new ArgumentException("Sweep angle is 0 or below");

            //renderTarget.AntialiasMode = AntialiasMode.Aliased;

            radius = Math.Max(radius, 1f);

            ArcSegment arcSegment = new ArcSegment();
            arcSegment.ArcSize = sweepAngle > 180f ? ArcSize.Large : ArcSize.Small;
            arcSegment.Point = PointOnCircle(center, radius, startAngle - (sweepAngle >= 360f ? 359.9f : sweepAngle));
            arcSegment.RotationAngle = 0f;
            arcSegment.Size = new Size2F(radius, radius);
            arcSegment.SweepDirection = SweepDirection.CounterClockwise;

            using (PathGeometry path = new PathGeometry(Factory))
            using (GeometrySink sink = path.Open())
            {
                sink.BeginFigure(PointOnCircle(center, radius, startAngle), FigureBegin.Filled);
                sink.AddArc(arcSegment);
                sink.EndFigure(FigureEnd.Open);

                sink.Close();

                renderTarget.DrawGeometry(path, brush, strokeWidth);
            }

            renderTarget.AntialiasMode = AntialiasMode.PerPrimitive;
        }

        /// <summary>
        /// Calcs a point on a circle
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="radius"></param>
        /// <param name="angleInDegrees"></param>
        /// <returns></returns>
        private static Vector2 PointOnCircle(Vector2 origin, float radius, float angleInDegrees)
        {
            // Convert from degrees to radians via multiplication by PI/180        
            float x = (float)(radius * Math.Cos(angleInDegrees * Math.PI / 180F)) + origin.X;
            float y = (float)(radius * Math.Sin(angleInDegrees * Math.PI / 180F)) + origin.Y;

            return new Vector2(x, y);
        }
    }
}
