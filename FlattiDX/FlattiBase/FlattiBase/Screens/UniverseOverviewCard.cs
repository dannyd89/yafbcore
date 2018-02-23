using FlattiBase.Brushes;
using FlattiBase.Fonts;
using FlattiBase.Forms;
using FlattiBase.Helper;
using FlattiBase.Interfaces;
using Flattiverse;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Screens
{
    public class UniverseOverviewCard : IFixedDrawable, IMouseListener, IDisposable
    {
        private Bitmap avatar;
        private string name;

        /// <summary>
        /// Name of the universegroup
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        private float X;
        private float Y;
        private float Width;
        private float Height;

        private bool hovered;
        private bool pressed;

        private RoundedRectangle roundedRectangle;
        private SharpDX.RectangleF rect;

        private SharpDX.RectangleF avatarRect;

        private const float PADDING = 3f;
        private const float WIDTH = 600f;
        private const float MAX_AVATAR_HEIGHT = 128f;
        private const float MAX_AVATAR_WIDTH = 128f;

        private Label universeNameLabel;

        public event EventHandler Click;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="universeGroup"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public UniverseOverviewCard(Screen parent, UniverseGroup universeGroup, float x, float y)
        {
            name = universeGroup.Name;

            avatar = BitmapConverter.ToSharpDXBitmap(parent.Parent.RenderTarget, universeGroup.Avatar);

            roundedRectangle = new RoundedRectangle();
            roundedRectangle.RadiusX = 8;
            roundedRectangle.RadiusY = 8;

            X = x;
            Y = y;
            Width = (parent.Parent.Width / 2f) - 40f;
            Height = avatar.PixelSize.Height > 128f ? MAX_AVATAR_HEIGHT + PADDING * 2f : avatar.PixelSize.Height + PADDING * 2f;

            rect = new SharpDX.RectangleF(x, y, Width, Height);
            roundedRectangle.Rect = rect;

            avatarRect = new SharpDX.RectangleF(x + PADDING, 
                                                y + PADDING,
                                                avatar.PixelSize.Width > 128f ? MAX_AVATAR_WIDTH: avatar.PixelSize.Width, 
                                                avatar.PixelSize.Height > 128f ? MAX_AVATAR_HEIGHT : avatar.PixelSize.Height);

            universeNameLabel = new Label(parent.Parent.DirectWriteFactory,
                                          name,
                                          FormFonts.HeadlineSmallFont,
                                          SolidColorBrushes.White,
                                          avatarRect.X + avatarRect.Width + 5f,
                                          y + PADDING,
                                          400f,
                                          19f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="renderTarget"></param>
        public void Draw(SharpDX.Direct2D1.RenderTarget renderTarget)
        {
            renderTarget.FillRoundedRectangle(roundedRectangle, SolidColorBrushes.Black);
            renderTarget.DrawRoundedRectangle(roundedRectangle, (hovered ? SolidColorBrushes.Yellow : SolidColorBrushes.White));

            renderTarget.DrawBitmap(avatar, avatarRect, 1.0f, BitmapInterpolationMode.NearestNeighbor);

            universeNameLabel.Draw(renderTarget);
        }

        public void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.X >= X 
                && e.Y >= Y 
                && e.X <= X + Width 
                && e.Y <= Y + Height)
                hovered = true;
            else
                hovered = false;
        }

        public void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.X >= X 
                && e.Y >= Y 
                && e.X <= X + Width 
                && e.Y <= Y + Height)
                pressed = true;
            else
                pressed = false;
        }

        public void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (Click != null 
                && pressed 
                && (e.X >= X 
                && e.Y >= Y 
                && e.X <= X + Width 
                && e.Y <= Y + Height))
                Click(this, new EventArgs());

            pressed = false;
        }

        public void Dispose()
        {
            if (avatar != null)
                avatar.Dispose();

            universeNameLabel.Dispose();
        }
    }
}
