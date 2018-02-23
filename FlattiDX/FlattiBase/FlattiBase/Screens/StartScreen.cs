using FlattiBase.Brushes;
using FlattiBase.Fonts;
using FlattiBase.Forms;
using FlattiBase.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Screens
{
    public class StartScreen : Screen
    {
        private Button connectButton;
        private Button closeButton;

        private Label headlineLabel;

        public StartScreen(ScreenManager parent)
            : base(parent, "StartScreen")
        {
            headlineLabel = new Label(parent.DirectWriteFactory, 
                                      "FLATTIVERSE",
                                      FormFonts.HeadlineLargeFont, 
                                      SolidColorBrushes.Red, 
                                      parent.Width / 2f, 
                                      100f,
                                      650f, 
                                      200f,
                                      true);

            connectButton = new Button(parent.DirectWriteFactory, 
                                       "Connect", 
                                       (parent.Width - Button.BUTTON_STANDARD_WIDTH) / 2f, 
                                       220f, 
                                       Button.BUTTON_STANDARD_WIDTH, 
                                       Button.BUTTON_STANDARD_HEIGHT);
            connectButton.Click += connectButton_Click;

            closeButton = new Button(parent.DirectWriteFactory, 
                                     "Close", 
                                     (parent.Width - Button.BUTTON_STANDARD_WIDTH) / 2f, 
                                     260f, 
                                     Button.BUTTON_STANDARD_WIDTH,
                                     Button.BUTTON_STANDARD_HEIGHT);
            closeButton.Click += closeButton_Click;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, EventArgs e)
        {
            parent.RequestClose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectButton_Click(object sender, EventArgs e)
        {
            parent.RemoveScreen(this);

            if (parent.Connector != null && parent.Connector.IsConnected)
            {
                parent.Connector.Close();
                parent.Connector = null;
            }
                        
            parent.Connector = new Flattiverse.Connector(Resources.EMail, Resources.Password);

            parent.AddScreen(new UniverseOverviewScreen(parent));
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
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastUpdate"></param>
        public override void Update(TimeSpan lastUpdate)
        {
            throw new NotImplementedException();
        }

        public override void Draw()
        {
            headlineLabel.Draw(parent.RenderTarget);

            connectButton.Draw(parent.RenderTarget);
            closeButton.Draw(parent.RenderTarget);
        }

        public override void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            connectButton.MouseMove(sender, e);
            closeButton.MouseMove(sender, e);
        }

        public override void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            connectButton.MouseDown(sender, e);
            closeButton.MouseDown(sender, e);
        }

        public override void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            connectButton.MouseUp(sender, e);
            closeButton.MouseUp(sender, e);
        }

        public override void MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public override void KeyPressed(System.Windows.Forms.Keys keyData)
        {
            //throw new NotImplementedException();
        }

        public override void Dispose()
        {
            headlineLabel.Dispose();

            connectButton.Dispose();
            closeButton.Dispose();
        }

    }
}
