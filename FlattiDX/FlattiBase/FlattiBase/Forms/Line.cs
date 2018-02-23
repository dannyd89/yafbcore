using FlattiBase.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Forms
{
    public class Line : IFixedDrawable
    {
        public Vector2 From;
        public Vector2 To;
        public float StrokeWidth;
        public Brush Color;

        public Line(Vector2 from, Vector2 to, Brush color, float strokeWidth = 1f)
        {
            From = from;
            To = to;
            Color = color;
            StrokeWidth = strokeWidth;
        }

        public void Draw(SharpDX.Direct2D1.RenderTarget renderTarget)
        {
            renderTarget.DrawLine(From, To, Color, StrokeWidth);
        }
    }
}
