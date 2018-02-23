using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Helper
{
    internal static class BitmapConverter
    {
        public static SharpDX.Direct2D1.Bitmap ToSharpDXBitmap(WindowRenderTarget renderTarget, System.Drawing.Bitmap bitmap)
        {
            if (bitmap == null)
                return null;

            BitmapData drawingBitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), 
                                                           ImageLockMode.ReadOnly, 
                                                           System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            DataStream dataStream = new DataStream(drawingBitmapData.Scan0, 
                                                   drawingBitmapData.Stride * drawingBitmapData.Height, 
                                                   true, false);

            BitmapProperties properties = new BitmapProperties()
            {
                PixelFormat = new SharpDX.Direct2D1.PixelFormat(Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied)
            };

            try
            {
                return new SharpDX.Direct2D1.Bitmap(renderTarget, new Size2(bitmap.Width, bitmap.Height), dataStream, drawingBitmapData.Stride, properties);
            }
            finally
            {
                bitmap.UnlockBits(drawingBitmapData);
            }
        }
    }
}
