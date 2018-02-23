using FlattiBase.Brushes;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Screens.MenuScreens
{
    public abstract class MenuScreen : Screen
    {
        public readonly Screen ParentScreen;

        protected readonly RenderTarget RenderTarget;
        protected readonly SharpDX.DirectWrite.Factory DirectWriteFactory;

        private SharpDX.RectangleF screenRectangle;

        public MenuScreen(Screen screen, string screenName)
            : base(screen.Parent, screenName)
        {
            ParentScreen = screen;
            RenderTarget = Parent.RenderTarget;
            DirectWriteFactory = Parent.DirectWriteFactory;

            screenRectangle = new SharpDX.RectangleF(0f, 0f, Parent.Width, Parent.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsUpdatable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsDrawable
        {
            get
            {
                return false;
            }
        }

        #region Mouse & Key Events
        public override void MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
        }

        public override void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
        }

        public override void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
        }

        public override void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
        }

        public override void KeyPressed(System.Windows.Forms.Keys keyData)
        {
            
        }
        #endregion


        public override void Update(TimeSpan lastUpdate)
        {
            
        }

        public override void Draw()
        {
            RenderTarget.FillRectangle(screenRectangle, SolidColorBrushes.BlackHalfTransparent);
        }

        public override void Dispose()
        {
            
        }
    }
}
