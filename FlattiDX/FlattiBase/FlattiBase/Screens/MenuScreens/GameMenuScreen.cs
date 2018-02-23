using FlattiBase.Brushes;
using FlattiBase.Fonts;
using FlattiBase.Forms;
using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Screens.MenuScreens
{
    public class GameMenuScreen : MenuScreen
    {
        //private List<Button> buttons;
        private Label captionLabel;

        private Button resumeButton;
        private Button leaveUniverseButton;
        private Button closeGameButton;

        private RoundedRectangle roundedRectangle;
        private RectangleF rect;

        private const int PADDING_X = 25;
        private const int PADDING_Y = 10;
        private const int PADDING_BUTTON_TEXT = 15;
        private const float BUTTON_WIDTH = Button.BUTTON_STANDARD_WIDTH + PADDING_BUTTON_TEXT;
        private float height;
        private float width;
        private float x;
        private float y;

        public GameMenuScreen(GameScreen screen)
            : base(screen, "GameMenuScreen")
        {
            width = PADDING_X * 2f + BUTTON_WIDTH;
            height = PADDING_Y * 5f + Button.BUTTON_STANDARD_HEIGHT * 4f;
            x = (Parent.Width - width) / 2f;
            y = (Parent.Height - height) / 2f;

            captionLabel = new Label(DirectWriteFactory, "Menu", FormFonts.HeadlineSmallFont, SolidColorBrushes.OrangeRed, x + width / 2f, y + PADDING_Y + 15f, 100f, 200f, true);

            int buttonCount = 1;
            resumeButton = new Button(DirectWriteFactory, "Resume Game", x + PADDING_X, y + PADDING_Y + ((Button.BUTTON_STANDARD_HEIGHT + PADDING_Y) * buttonCount++), BUTTON_WIDTH);
            resumeButton.Click += resumeButton_Click;

            leaveUniverseButton = new Button(DirectWriteFactory, "Leave Universe", x + PADDING_X, y + PADDING_Y + ((Button.BUTTON_STANDARD_HEIGHT + PADDING_Y) * buttonCount++), BUTTON_WIDTH);
            leaveUniverseButton.Click += leaveUniverseButton_Click;

            closeGameButton = new Button(DirectWriteFactory, "Exit Game", x + PADDING_X, y + PADDING_Y + ((Button.BUTTON_STANDARD_HEIGHT + PADDING_Y) * buttonCount++), BUTTON_WIDTH);
            closeGameButton.Click += closeGameButton_Click;

            roundedRectangle = new RoundedRectangle();
            roundedRectangle.RadiusX = 8;
            roundedRectangle.RadiusY = 8;

            rect = new RectangleF(x, y, width, height);
            roundedRectangle.Rect = rect;
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

        private void resumeButton_Click(object sender, EventArgs e)
        {
            Parent.RemoveScreen(this);

            ((GameScreen)ParentScreen).ShowGameMenu = false;
        }

        private void leaveUniverseButton_Click(object sender, EventArgs e)
        {
            ((GameScreen)ParentScreen).LeaveCurrentUniverse();

            ((GameScreen)ParentScreen).ShowGameMenu = false;

            Parent.RemoveScreen(this);
        }

        private void closeGameButton_Click(object sender, EventArgs e)
        {
            Parent.RequestClose();
        }

        public override void MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
        }

        public override void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            resumeButton.MouseMove(sender, e);
            leaveUniverseButton.MouseMove(sender, e);
            closeGameButton.MouseMove(sender, e);
        }

        public override void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            resumeButton.MouseDown(sender, e);
            leaveUniverseButton.MouseDown(sender, e);
            closeGameButton.MouseDown(sender, e);
        }

        public override void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            resumeButton.MouseUp(sender, e);
            leaveUniverseButton.MouseUp(sender, e);
            closeGameButton.MouseUp(sender, e);
        }

        public override void KeyPressed(System.Windows.Forms.Keys keyData)
        {
            if (keyData == System.Windows.Forms.Keys.Escape)
            {
                Parent.RemoveScreen(this);

                ((GameScreen)ParentScreen).ShowGameMenu = false;
            }
        }

        public override void Update(TimeSpan lastUpdate)
        {
            //throw new NotImplementedException();
        }

        public override void Draw()
        {
            base.Draw();

            RenderTarget.FillRoundedRectangle(roundedRectangle, SolidColorBrushes.Black);
            RenderTarget.DrawRoundedRectangle(roundedRectangle, SolidColorBrushes.White);

            captionLabel.Draw(RenderTarget);

            resumeButton.Draw(RenderTarget);
            leaveUniverseButton.Draw(RenderTarget);
            closeGameButton.Draw(RenderTarget);
        }

        public override void Dispose()
        {
            resumeButton.Dispose();
            leaveUniverseButton.Dispose();
            closeGameButton.Dispose();
        }
    }
}
