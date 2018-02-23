using FlattiBase.Brushes;
using FlattiBase.Fonts;
using FlattiBase.Forms;
using FlattiBase.Forms.TableComponents;
using FlattiBase.Managers;
using Flattiverse;
using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Screens
{
    public class UniverseLobbyScreen : Screen
    {
        private readonly UniverseGroup universeGroup;

        private Label headlineLabel;
        private UniverseTable universeTable;
        private List<Button> teamButtons;
        private Button backButton;

        private const int PADDING = 20;
        private const int PADDING_TOP = 50;

        public UniverseLobbyScreen(ScreenManager parent)
            : base(parent, "UniverseLobbyScreen")
        {
            universeGroup = parent.UniverseGroup;

            headlineLabel = new Label(parent.DirectWriteFactory,
                                      universeGroup.Name.Replace('g', 'G'),
                                      FormFonts.HeadlineMediumFont,
                                      SolidColorBrushes.Red,
                                      PADDING,
                                      PADDING_TOP,
                                      600f,
                                      40f);

            teamButtons = new List<Button>();

            Button teamButton;
            int teamCount = universeGroup.Teams.Count<Team>();
            foreach (Team team in universeGroup.Teams)
            {
                SolidColorBrush teamColor = new SolidColorBrush(parent.RenderTarget, new Color4(team.Red, team.Green, team.Blue, 1f));

                if (!SolidColorBrushes.TeamColors.ContainsKey(team.Name))
                    SolidColorBrushes.TeamColors.Add(team.Name, teamColor);

                teamButton = new Button(parent.DirectWriteFactory, 
                                        "Join " + team.Name, 
                                        parent.Width - ((Button.BUTTON_STANDARD_WIDTH + PADDING) * (teamCount - teamButtons.Count)), 
                                        parent.Height - Button.BUTTON_STANDARD_HEIGHT - PADDING);

                teamButton.TextColor = teamColor;
                teamButton.Tag = team;
                teamButton.Click += teamButton_Click;

                teamButtons.Add(teamButton);
            }

            universeTable = new UniverseTable(this, universeGroup, PADDING, PADDING_TOP + 45f, parent.Width - PADDING * 2f, 800f, SolidColorBrushes.Black);

            universeTable.AddColumn(" ", "SmallAvatar", UniverseTeamTable.MAX_COLUMN_WIDTH, UniverseTeamTable.MAX_COLUMN_HEIGHT);
            universeTable.AddColumn("Name", "Name",  UniverseTeamTable.MAX_COLUMN_WIDTH, UniverseTeamTable.MAX_COLUMN_HEIGHT);
            universeTable.AddColumn("Score", "Score",  UniverseTeamTable.MAX_COLUMN_WIDTH, UniverseTeamTable.MAX_COLUMN_HEIGHT);
            universeTable.AddColumn("Kills", "Kills",  UniverseTeamTable.MAX_COLUMN_WIDTH, UniverseTeamTable.MAX_COLUMN_HEIGHT);
            universeTable.AddColumn("Deaths", "Deaths", UniverseTeamTable.MAX_COLUMN_WIDTH, UniverseTeamTable.MAX_COLUMN_HEIGHT);
            universeTable.AddColumn("Ping", "Ping", UniverseTeamTable.MAX_COLUMN_WIDTH, UniverseTeamTable.MAX_COLUMN_HEIGHT);

            backButton = new Button(parent.DirectWriteFactory, "Back", PADDING, parent.Height - Button.BUTTON_STANDARD_HEIGHT - PADDING);
            backButton.Click += backButton_Click;
        }

        private void teamButton_Click(object sender, EventArgs e)
        {
            Button teamButton = sender as Button;
            Team team = teamButton.Tag as Team;

            if (!parent.UniverseGroup.PasswordRequired)
                parent.UniverseGroup.Join("dannyDX", team);
            else
            {
                parent.UniverseGroup.Join("dannyDX", team, "md", "l%har256");
                // TODO
                // Game message
                //return;
            }

            Parent.RemoveScreen(this);
            Parent.AddScreen(new GameScreen(Parent, team));
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            parent.RemoveScreen(this);
            parent.AddScreen(new UniverseOverviewScreen(Parent));
        }

        public override void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            foreach (Button teamButton in teamButtons)
                teamButton.MouseMove(sender, e);

            backButton.MouseMove(sender, e);
        }

        public override void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            foreach (Button teamButton in teamButtons)
                teamButton.MouseDown(sender, e);

            backButton.MouseDown(sender, e);
        }

        public override void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            foreach (Button teamButton in teamButtons)
                teamButton.MouseUp(sender, e);

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
                return true;
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
            universeTable.Update(lastUpdate);
        }

        public override void Draw()
        {
            headlineLabel.Draw(parent.RenderTarget);
            universeTable.Draw(parent.RenderTarget);

            foreach (Button teamButton in teamButtons)
                teamButton.Draw(parent.RenderTarget);

            backButton.Draw(parent.RenderTarget);
        }

        public override void Dispose()
        {
            headlineLabel.Dispose();
            universeTable.Dispose();

            foreach (var button in teamButtons)
                button.Dispose();

            backButton.Dispose();
        }



    }
}
