using System;
using System.Windows.Forms;
using System.Diagnostics;
using SharpDX.Windows;
using SharpDX.Direct2D1;
using FlattiBase.Helper;
using FlattiBase.Colors;
using FlattiBase.Brushes;
using FlattiBase.Fonts;
using FlattiBase.Managers;
using FlattiBase.Simples;
using SharpDX;

namespace FlattiUI
{
    public partial class GameUI : RenderForm
    {
        public static WindowRenderTarget RenderTarget;
        public static SharpDX.Direct2D1.Factory Factory;
        public static SharpDX.DirectWrite.Factory DirectWriteFactory;
        public static ScreenManager ScreenManager;

        private Stopwatch time;

        private int fps = 0;

        private int currentFps = 0;

        public GameUI()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.Opaque, true);

            MouseWheel += GameUI_MouseWheel;
        }

        public void SetWindowRenderTarget(WindowRenderTarget renderTarget)
        {
            RenderTarget = renderTarget;

            //d2dContext = renderTarget.QueryInterface<DeviceContext>();
        }

        public void SetFactory(SharpDX.Direct2D1.Factory factory)
        {
            Factory = factory;
        }

        public void SetDirectWriteFactory(SharpDX.DirectWrite.Factory directWriteFactory)
        {
            DirectWriteFactory = directWriteFactory;
        }

        public void EnterFullScreenMode()
        {
            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }

        public void LeaveFullScreenMode()
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Normal;
        }

        private void GameUI_Load(object sender, EventArgs e)
        {
            //RenderTarget.AntialiasMode = AntialiasMode.Aliased;

            // Init everything before creating a screen manager
            SolidColorBrushes.Init(RenderTarget);
            FormFonts.Init(DirectWriteFactory);
            FlattiBase.Simples.Arc.Factory = Factory;

            // Go fullscreen mode
            //EnterFullScreenMode();

            ScreenManager = new FlattiBase.Managers.ScreenManager(this, RenderTarget, Factory, DirectWriteFactory, ClientSize.Width, ClientSize.Height);
            ScreenManager.RequestingClose += ScreenManager_RequestingClose;

            time = Stopwatch.StartNew();
        }

        public void Render()
        {
            RenderTarget.BeginDraw();
            RenderTarget.Clear(AdvancedColors.DarkGray);

            ScreenManager.UpdateAndDraw();

            //BitmapRenderTarget brt = new BitmapRenderTarget(RenderTarget, CompatibleRenderTargetOptions.None, new PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied));
            //brt.BeginDraw();
            //RenderTarget.DrawText(currentFps.ToString() + " Fps", FormFonts.ColumnHeaderFont, new SharpDX.RectangleF(5f, 5f, 150f, 80f), SolidColorBrushes.White);
            //brt.EndDraw();

            //RenderTarget.DrawBitmap(brt.Bitmap, 0.5f, BitmapInterpolationMode.Linear);

            RenderTarget.EndDraw();

            //Log.AddLogEntry("FPS", new TimeSpan(currentFps, 0, 0));

            //if (time.ElapsedMilliseconds >= 1000)
            //{
            //    time.Restart();
            //    currentFps = fps;
            //    fps = 0;
            //}

            //fps++;
        }

        private void GameUI_Resize(object sender, EventArgs e)
        {
            if (RenderTarget != null)
                RenderTarget.Resize(new SharpDX.Size2(this.ClientSize.Width, this.ClientSize.Height));

            if (ScreenManager != null)
            {
                ScreenManager.Width = ClientSize.Width;
                ScreenManager.Height = ClientSize.Height;
            }
        }

        private void GameUI_KeyDown(object sender, KeyEventArgs e)
        {
            if (ScreenManager != null)
                ScreenManager.KeyPressed(e.KeyData);

            if (e.Alt && e.KeyCode == Keys.Enter)
                EnterFullScreenMode();
        }

        private void GameUI_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ScreenManager != null)
                ScreenManager.MouseWheel(sender, e);
        }

        private void GameUI_MouseMove(object sender, MouseEventArgs e)
        {
            if (ScreenManager != null)
                ScreenManager.MouseMove(sender, e);
        }

        private void GameUI_MouseDown(object sender, MouseEventArgs e)
        {
            if (ScreenManager != null)
                ScreenManager.MouseDown(sender, e);
        }

        private void GameUI_MouseUp(object sender, MouseEventArgs e)
        {
            if (ScreenManager != null)
                ScreenManager.MouseUp(sender, e);
        }

        private void ScreenManager_RequestingClose(object sender, EventArgs e)
        {
            Close();
        }

        private void GameUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            LeaveFullScreenMode();

            ScreenManager.Dispose();

            //if (ScreenManager.Connector != null)
            //    ScreenManager.Connector.Close();

            Log.ExportLogs();
        }
    }
}
