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
    public class ImageBox : IFixedDrawable
    {
        public float X;
        public float Y;
        public readonly SharpDX.Direct2D1.Bitmap Bitmap;
        public RectangleF Position;

        public ImageBox(SharpDX.Direct2D1.Bitmap bitmap, float x, float y)
        {
            Bitmap = bitmap;
            X = x;
            Y = y;

            Position = new RectangleF(x, y, bitmap.PixelSize.Width, bitmap.PixelSize.Height);
        }

        public void Draw(SharpDX.Direct2D1.RenderTarget renderTarget)
        {
            renderTarget.DrawBitmap(Bitmap, Position, 1f, BitmapInterpolationMode.Linear);
        }
    }
}
