using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Windows;
using SharpDX.DXGI;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;

namespace FlattiUI
{
    static class Program
    {
        private static WindowRenderTarget wndRender = null;
        private static SharpDX.Direct2D1.Factory fact = new SharpDX.Direct2D1.Factory(FactoryType.SingleThreaded);
        private static SharpDX.DirectWrite.Factory directWriteFactory = new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated);
        private static RenderTargetProperties rndTargProperties;
        private static HwndRenderTargetProperties hwndProperties;
        private static GameUI form;

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            form = new GameUI();

            rndTargProperties = new RenderTargetProperties(new PixelFormat(Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied));

            hwndProperties = new HwndRenderTargetProperties();
            hwndProperties.Hwnd = form.Handle;
            hwndProperties.PixelSize = new SharpDX.Size2(form.ClientSize.Width, form.ClientSize.Height);
            hwndProperties.PresentOptions = PresentOptions.None;
            wndRender = new WindowRenderTarget(fact, rndTargProperties, hwndProperties);

            

            form.SetWindowRenderTarget(wndRender);
            form.SetFactory(fact);
            form.SetDirectWriteFactory(directWriteFactory);

            RenderLoop.Run(form, form.Render);
        }
    }
}
