using FlattiBase.Brushes;
using FlattiBase.Fonts;
using FlattiBase.Forms;
using FlattiBase.Managers;
using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Screens
{
    public class UniverseOverviewScreen : Screen
    {
        private Label headlineLabel;
        private List<UniverseOverviewCard> universeOverviews;

        private Button backButton;

        private const int PADDING = 20;

        public UniverseOverviewScreen(ScreenManager parent)
            : base(parent, "UniverseOverviewScreen")
        {
            headlineLabel = new Label(parent.DirectWriteFactory,
                                      "CHOOSE A UNIVERSE",
                                      FormFonts.HeadlineMediumFont,
                                      SolidColorBrushes.Red,
                                      20f,
                                      100f,
                                      650f,
                                      200f);

            universeOverviews = new List<UniverseOverviewCard>();

            float startHeight = 140f;

            UniverseOverviewCard universeOverview;
            foreach (UniverseGroup universeGroup in parent.Connector.UniverseGroups)
            {
                universeOverview = new UniverseOverviewCard(this, universeGroup, PADDING + (parent.Width / 2f) * (universeOverviews.Count % 2), startHeight + 140f * (int)(universeOverviews.Count / 2));
                universeOverview.Click += universeOverview_Click;

                universeOverviews.Add(universeOverview);
            }

            backButton = new Button(parent.DirectWriteFactory, "Back", PADDING, parent.Height - Button.BUTTON_STANDARD_HEIGHT - PADDING);
            backButton.Click += backButton_Click;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            parent.RemoveScreen(this);

            parent.AddScreen(new StartScreen(parent));
        }

        private void universeOverview_Click(object sender, EventArgs e)
        {
            UniverseOverviewCard universeOverview = sender as UniverseOverviewCard;

            parent.UniverseGroup = parent.Connector.UniverseGroups[universeOverview.Name];

            parent.RemoveScreen(this);

            parent.AddScreen(new UniverseLobbyScreen(parent));
        }

        public override void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            foreach (UniverseOverviewCard universeOverview in universeOverviews)
                universeOverview.MouseMove(sender, e);

            backButton.MouseMove(sender, e);
        }

        public override void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            foreach (UniverseOverviewCard universeOverview in universeOverviews)
                universeOverview.MouseDown(sender, e);

            backButton.MouseDown(sender, e);
        }

        public override void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            foreach (UniverseOverviewCard universeOverview in universeOverviews)
                universeOverview.MouseUp(sender, e);

            backButton.MouseUp(sender, e);
        }

        public override void MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public override void KeyPressed(System.Windows.Forms.Keys keyData)
        {
            //throw new NotImplementedException();
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

        public override void Update(TimeSpan lastUpdate)
        {
            throw new NotImplementedException();
        }

        public override void Draw()
        {
            headlineLabel.Draw(parent.RenderTarget);

            foreach (UniverseOverviewCard universeOverview in universeOverviews)
                universeOverview.Draw(parent.RenderTarget);

            backButton.Draw(parent.RenderTarget);
        }

        public override void Dispose()
        {
            headlineLabel.Dispose();
            foreach (var universeOverview in universeOverviews)
                universeOverview.Dispose();

            backButton.Dispose();
        }

        
    }
}
