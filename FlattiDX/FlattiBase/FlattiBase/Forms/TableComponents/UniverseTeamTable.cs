using FlattiBase.Brushes;
using FlattiBase.Fonts;
using FlattiBase.Helper;
using FlattiBase.Interfaces;
using FlattiBase.Screens;
using Flattiverse;
using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Forms.TableComponents
{
    public class UniverseTeamTable : IFixedDrawable, IDisposable
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;
        public float MaxWidth;
        public float MaxHeight;

        private readonly Screen screen;
        private readonly Team team;
        private readonly SolidColorBrush teamColor;

        private Label teamNameLabel;
        public List<Column> Columns;
        public List<Row> Rows;
        private Line columnLine;

        public const int PADDING = 5;
        public const int MAX_COLUMN_WIDTH = 200;
        public const int MAX_COLUMN_HEIGHT = 20;

        public UniverseTeamTable(Screen screen, Team team, SolidColorBrush teamColor)
        {
            this.screen = screen;
            this.team = team;
            this.teamColor = teamColor;

            Columns = new List<Column>();
            Rows = new List<Row>();
        }

        public void Update(List<Player> players, float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            MaxWidth = width;
            MaxHeight = height;

            Height = 0f;

            SharpDX.DirectWrite.Factory directWriteFactory = screen.Parent.DirectWriteFactory;

            if (teamNameLabel != null)
            {
                teamNameLabel.Dispose();
                teamNameLabel = null;
            }

            teamNameLabel = new Label(directWriteFactory,
                                      "Team " + team.Name,
                                      FormFonts.HeadlineSmallFont,
                                      teamColor,
                                      x + PADDING,
                                      y + PADDING,
                                      300f,
                                      25f);

            Height += PADDING + teamNameLabel.TextLayout.Metrics.Height;

            float columnWidth = 0f;
            for (int i = 0; i < Columns.Count; i++)
            {
                Columns[i].Position = new Vector2(x + PADDING + columnWidth, y + Height);

                columnWidth += Columns[i].Width;
            }

            Height += MAX_COLUMN_HEIGHT;

            columnLine = new Line(new Vector2(x + PADDING, y + Height), new Vector2(x - PADDING + MaxWidth, y + Height), teamColor, 2f);

            Height += columnLine.StrokeWidth;

            if (Rows != null)
                foreach (Row r in Rows)
                    r.Dispose();

            Rows = new List<Row>();

            Row row;
            foreach (Player player in players)
            {
                row = new Row(directWriteFactory, teamColor, this, new Data.PlayerEntry(screen.Parent.RenderTarget, player), x, y + Height, width, height);

                Height += row.Height;

                Rows.Add(row);
            }
        }

        public void Draw(RenderTarget renderTarget)
        {
            teamNameLabel.Draw(renderTarget);

            foreach (Column column in Columns)
                column.Draw(renderTarget, teamColor);

            columnLine.Draw(renderTarget);

            foreach (Row row in Rows)
                row.Draw(renderTarget);
        }

        public void Dispose()
        {
            teamNameLabel.Dispose();

            foreach (Column column in Columns)
                column.Dispose();

            foreach (var row in Rows)
	            row.Dispose();
        }
    }
}
