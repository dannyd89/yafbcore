using FlattiBase.Data;
using FlattiBase.Helper;
using FlattiBase.Screens;
using Flattiverse;
using SharpDX.Direct2D1;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Managers
{
    public class ScreenManager : IDisposable
    {
        public readonly WindowRenderTarget RenderTarget;
        public readonly SharpDX.Direct2D1.Factory Factory;
        public readonly SharpDX.DirectWrite.Factory DirectWriteFactory;
        public readonly RenderForm ParentForm;

        public Connector Connector;
        public UniverseGroup UniverseGroup;

        private List<Screen> drawableScreens;
        private List<Screen> updatableScreens;
        private TimeSpan lastUpdate;

        public float Width;
        public float Height;

        public event EventHandler RequestingClose;

        public ScreenManager(RenderForm parent, WindowRenderTarget renderTarget, SharpDX.Direct2D1.Factory factory, SharpDX.DirectWrite.Factory directWriteFactory, float width, float height)
        {
            ParentForm = parent;
            RenderTarget = renderTarget;
            Factory = factory;
            DirectWriteFactory = directWriteFactory;

            Width = width;
            Height = height;

            if (System.IO.File.Exists("benchmark.bin"))
            {
                try
                {
                    using (System.IO.FileStream fileStream = new System.IO.FileStream("benchmark.bin", System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        byte[] fileBuffer = new byte[fileStream.Length];
                        fileStream.Read(fileBuffer, 0, fileBuffer.Length);
                        Connector.LoadBenchmark(fileBuffer);
                    }
                }
                catch
                {
                    System.IO.File.Delete("benchmark.bin");

                    Connector.DoBenchmark();

                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(Connector.SaveBenchmark()))
                    using (System.IO.FileStream fileStream = new System.IO.FileStream("benchmark.bin", System.IO.FileMode.Create, System.IO.FileAccess.Write))
                        memoryStream.WriteTo(fileStream);
                }
            }
            else
            {
                Connector.DoBenchmark();

                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(Connector.SaveBenchmark()))
                using (System.IO.FileStream fileStream = new System.IO.FileStream("benchmark.bin", System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    memoryStream.WriteTo(fileStream);
            }

            ImageCollections.PlayerDefaultSmallAvatar = BitmapConverter.ToSharpDXBitmap(renderTarget, Resources._default);

            PropertyInfo[] propertyInfos = typeof(PlayerEntry).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            PlayerEntry.PropertyInfos = new Dictionary<string, PropertyInfo>();

            for (int i = 0; i < propertyInfos.Length; i++)
                PlayerEntry.PropertyInfos.Add(propertyInfos[i].Name, propertyInfos[i]);

            drawableScreens = new List<Screen>();
            updatableScreens = new List<Screen>();

            AddScreen(new StartScreen(this));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screen"></param>
        internal void AddScreen(Screen screen)
        {
            if (screen.IsUpdatable && !updatableScreens.Contains(screen))
                updatableScreens.Add(screen);

            if (screen.IsDrawable && !drawableScreens.Contains(screen))
                drawableScreens.Add(screen);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screen"></param>
        /// <returns></returns>
        internal void RemoveScreen(Screen screen)
        {
            screen.Dispose();

            if (screen.IsDrawable)
                drawableScreens.Remove(screen);

            if (screen.IsUpdatable)
                updatableScreens.Remove(screen);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void RequestClose()
        {
            if (RequestingClose != null)
                RequestingClose(this, new EventArgs());
        }

        #region Event functions
        /// <summary>
        /// Called if the mouse wheel was moved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            List<Screen> tempScreens = new List<Screen>(drawableScreens);

            foreach (Screen screen in tempScreens)
                screen.MouseWheel(sender, e);
        }

        /// <summary>
        /// Called if the mouse was moved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            List<Screen> tempScreens = new List<Screen>(drawableScreens);

            foreach (Screen screen in tempScreens)
                screen.MouseMove(sender, e);
        }

        /// <summary>
        /// Called if a mouse button is pressed down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            List<Screen> tempScreens = new List<Screen>(drawableScreens);

            foreach (Screen screen in tempScreens)
                screen.MouseDown(sender, e);
        }

        /// <summary>
        /// Called if a mouse button is released
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            List<Screen> tempScreens = new List<Screen>(drawableScreens);

            foreach (Screen screen in tempScreens)
                screen.MouseUp(sender, e);
        }

        public void KeyPressed(System.Windows.Forms.Keys keyData)
        {
            List<Screen> tempScreens = new List<Screen>(drawableScreens);

            foreach (Screen screen in tempScreens)
                screen.KeyPressed(keyData);
        }

        /// <summary>
        /// Gets called if the render target is resized
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Resize(float width, float height)
        {
            Width = width;
            Height = height;
        }
        #endregion

        /// <summary>
        /// Updates all subscribed screens and draws them
        /// </summary>
        public void UpdateAndDraw()
        {
            DateTime start = DateTime.Now;

            if (updatableScreens.Count > 0)
                foreach (Screen screen in updatableScreens)
                    screen.Update(lastUpdate);

            if (drawableScreens.Count > 0)
                foreach (Screen screen in drawableScreens)
                    screen.Draw();

            //Console.WriteLine("Updating " + updatableScreens.Count.ToString() + " Screens and Drawing " + drawableScreens.Count.ToString() + " Screens");

            lastUpdate = DateTime.Now - start;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            for (int i = drawableScreens.Count - 1; i >= 0; i--)
            {
                drawableScreens[i].Dispose();

                drawableScreens.RemoveAt(i);
            }

            if (UniverseGroup != null && Connector.Player.UniverseGroup != null)
            {
                UniverseGroup.Part();

                UniverseGroup = null;
            }

            if (Connector != null && Connector.IsConnected)
            {
                Connector.Close();

                Connector = null;
            }
        }
    }
}
