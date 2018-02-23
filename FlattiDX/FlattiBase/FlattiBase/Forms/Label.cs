using FlattiBase.Fonts;
using FlattiBase.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Forms
{
    public class Label : IFixedDrawable, IDisposable
    {
        public Vector2 TextPosition;
        public readonly TextLayout TextLayout;
        public SolidColorBrush TextColor;
        public readonly bool Centered;
        public readonly string Text;
        public readonly RectangleF TextLayoutRect;
        public readonly TextFormat TextFormat;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directWriteFactory"></param>
        /// <param name="text"></param>
        /// <param name="textFormat"></param>
        /// <param name="textColor"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="center"></param>
        public Label(SharpDX.DirectWrite.Factory directWriteFactory, string text, TextFormat textFormat, SolidColorBrush textColor, float x, float y, float width, float height, bool center = false)
        {
            TextColor = textColor;
            Text = text;
            Centered = center;
            TextFormat = textFormat;
            TextLayout = new TextLayout(directWriteFactory, text, textFormat, width, height);
            TextLayout.ParagraphAlignment = ParagraphAlignment.Near;
            TextMetrics textMetrics = TextLayout.Metrics;
            
            if (center)
                TextPosition = new Vector2(x - (textMetrics.Width / 2f), y - (textMetrics.Height / 2f));
            else
                TextPosition = new Vector2(x, y);
        }

        public void Draw(SharpDX.Direct2D1.RenderTarget renderTarget)
        {
            renderTarget.DrawTextLayout(TextPosition, TextLayout, TextColor);
        }

        public void Dispose()
        {         
            TextLayout.Dispose();
        }
    }
}
