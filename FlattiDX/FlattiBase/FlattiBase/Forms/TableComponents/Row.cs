using FlattiBase.Data;
using FlattiBase.Fonts;
using FlattiBase.Helper;
using FlattiBase.Interfaces;
using Flattiverse;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Forms.TableComponents
{
    public class Row : IFixedDrawable, IUpdateable, IDisposable
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        private List<IFixedDrawable> rowContent;
        private List<IDisposable> disposables;
        private Line rowLine;

        public Row(SharpDX.DirectWrite.Factory directWriteFactory, SolidColorBrush textColor, UniverseTeamTable teamTable, PlayerEntry playerEntry, float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;

            rowContent = new List<IFixedDrawable>();
            disposables = new List<IDisposable>();

            Height = 0f;

            try
            {
                float columnWidth = 0f;
                foreach (Column column in teamTable.Columns)
                {
                    if (PlayerEntry.PropertyInfos[column.FieldName].PropertyType == typeof(SharpDX.Direct2D1.Bitmap))
                    {
                        Bitmap bitmap = (SharpDX.Direct2D1.Bitmap)PlayerEntry.PropertyInfos[column.FieldName].GetValue(playerEntry);

                        ImageBox imageBox = new ImageBox(bitmap,
                                                x + UniverseTeamTable.PADDING + columnWidth,
                                                y);

                        if (imageBox.Bitmap.PixelSize.Height > Height)
                            Height = imageBox.Bitmap.PixelSize.Height;

                        rowContent.Add(imageBox);
                    }
                    else
                    {
                        Label label = new Label(directWriteFactory,
                                          PlayerEntry.PropertyInfos[column.FieldName].GetValue(playerEntry).ToString(),
                                          FormFonts.NormalTextFont,
                                          textColor,
                                          x + UniverseTeamTable.PADDING + columnWidth,
                                          y,
                                          width, height);

                        disposables.Add(label);

                        if (label.TextLayout.Metrics.Height > Height)
                            Height = label.TextLayout.Metrics.Height;

                        rowContent.Add(label);
                    }

                    columnWidth += column.Width;
                }
            }
            catch
            { }

            rowLine = new Line(new Vector2(x + UniverseTeamTable.PADDING, y + Height), new Vector2(x - UniverseTeamTable.PADDING + Width, y + Height), textColor);
        }

        public void Draw(SharpDX.Direct2D1.RenderTarget renderTarget)
        {
            foreach (IFixedDrawable drawable in rowContent)
                drawable.Draw(renderTarget);

            rowLine.Draw(renderTarget);
        }

        public void Update(TimeSpan lastTime)
        {
            //throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (disposables != null)
                foreach (var disposable in disposables)
                    disposable.Dispose();
        }
    }
}
