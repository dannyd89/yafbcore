using FlattiBase.Brushes;
using FlattiBase.Data;
using FlattiBase.Fonts;
using FlattiBase.Interfaces;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Forms.TableComponents
{
    public class Column : IDisposable
    {
        public Vector2 Position;
        public float Width;
        public float Height;
        public readonly string Caption;
        public readonly string FieldName;

        public readonly TextLayout TextLayout;

        public Column(SharpDX.DirectWrite.Factory directWriteFactory, string caption, string fieldName, float width, float height)
        {
            Caption = caption;
            FieldName = fieldName;
            Width = width;
            Height = height;

            TextLayout = new TextLayout(directWriteFactory, caption, FormFonts.ColumnHeaderFont, width, height);
            TextMetrics textMetrics = TextLayout.Metrics;
            TextLayout.ParagraphAlignment = ParagraphAlignment.Near;
        }

        public void Draw(SharpDX.Direct2D1.RenderTarget renderTarget, SolidColorBrush textColor)
        {
            renderTarget.DrawTextLayout(Position, TextLayout, textColor);
        }

        public void Dispose()
        {
            TextLayout.Dispose();
        }
    }
}
