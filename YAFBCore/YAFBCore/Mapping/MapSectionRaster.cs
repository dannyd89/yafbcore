using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAFBCore.Mapping
{
    internal class MapSectionRaster
    {
        internal readonly MapSection MapSection;
        internal readonly MapSectionRasterTile[] Raster;
        internal readonly Size Size;

        internal MapSectionRaster(MapSection mapSection, MapSectionRasterTile[] raster, Size size)
        {
            MapSection = mapSection;
            Raster = raster;
            Size = size;
        }
    }
}
