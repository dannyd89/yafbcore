using FlattiBase.Brushes;
using FlattiBase.Fonts;
using FlattiBase.Helper;
using FlattiBase.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlattiBase.Forms
{
    public class Button : IFixedDrawable, IDisposable
    {
        public string Caption;
        public float X;
        public float Y;
        
        public float Width;
        public float Height;

        public SolidColorBrush TextColor;

        private RoundedRectangle roundedRectangle;
        private SharpDX.RectangleF rect;
        private Vector2 textPosition;
        private TextLayout textLayout;

        private bool hovered;
        private bool pressed;

        public const float BUTTON_STANDARD_WIDTH = 180f;
        public const float BUTTON_STANDARD_HEIGHT = 36f;

        public object Tag;

        #region Events
        public event EventHandler Click;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directWriteFactory"></param>
        /// <param name="caption"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Button(SharpDX.DirectWrite.Factory directWriteFactory, string caption, float x, float y, float width = BUTTON_STANDARD_WIDTH, float height = BUTTON_STANDARD_HEIGHT)
        {
            Caption = caption;
            X = x;
            Y = y;           
            Width = width;
            Height = height;

            hovered = false;
            pressed = false;

            TextColor = SolidColorBrushes.White;

            roundedRectangle = new RoundedRectangle();
            roundedRectangle.RadiusX = 8;
            roundedRectangle.RadiusY = 8;

            rect = new RectangleF(x, y, width, height);
            roundedRectangle.Rect = rect;

            textLayout = new TextLayout(directWriteFactory, caption, FormFonts.ButtonNormalFont, width, height);
            TextMetrics textMetrics = textLayout.Metrics;
            textPosition = new Vector2(x + ((width / 2f) - (textMetrics.Width / 2f)), y + ((height / 2f) - (textMetrics.Height / 2f)));
        }

        /// <summary>
        /// Draws the button
        /// </summary>
        /// <param name="renderTarget">Rendertarget need</param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public void Draw(RenderTarget renderTarget)
        {
            renderTarget.FillRoundedRectangle(roundedRectangle, SolidColorBrushes.Black);
            renderTarget.DrawRoundedRectangle(roundedRectangle, (pressed ? SolidColorBrushes.Red : (hovered ? SolidColorBrushes.Yellow : SolidColorBrushes.White)));

            renderTarget.DrawTextLayout(textPosition, textLayout, TextColor);
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X >= X && e.Y >= Y && e.X <= X + Width && e.Y <= Y + Height)
                hovered = true;
            else
                hovered = false;
        }

        public void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X >= X && e.Y >= Y && e.X <= X + Width && e.Y <= Y + Height)
                pressed = true;
            else
                pressed = false;
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            if (Click != null && pressed && (e.X >= X && e.Y >= Y && e.X <= X + Width && e.Y <= Y + Height))
                Click(this, new EventArgs());

             pressed = false;
        }

        public void Dispose()
        {
            textLayout.Dispose();
        }
    }
}
