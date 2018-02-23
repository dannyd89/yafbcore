using FlattiBase.Brushes;
using FlattiBase.Helper;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Mapping
{
    public class MapRaster
    {
        internal readonly Map Map;
        internal readonly MapRasterTile[] Raster;
        internal readonly Size Size;

        internal MapRaster(Map map, MapRasterTile[] raster, Size size)
        {
            Map = map;
            Raster = raster;
            Size = size;
        }

        internal void Draw(RenderTarget renderTarget, Transformator X, Transformator Y)
        {
            foreach (MapRasterTile tile in Raster)
                renderTarget.DrawRectangle(new SharpDX.RectangleF(X[tile.X], Y[tile.Y], X.Prop(tile.Size), Y.Prop(tile.Size)), SolidColorBrushes.CadetBlue);
        }
    }
}
