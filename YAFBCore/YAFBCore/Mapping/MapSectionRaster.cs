using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAFBCore.Mapping.Units;
using YAFBCore.Utils;
using YAFBCore.Utils.Mathematics;

namespace YAFBCore.Mapping
{
    internal class MapSectionRaster
    {
        internal readonly MapSection MapSection;
        internal readonly MapSectionRasterTile[] Raster;
        internal readonly int Width;
        internal readonly int Height;

        private readonly Transformator X;
        private readonly Transformator Y;

        internal MapSectionRaster(MapSection mapSection, MapSectionRasterTile[] raster, int size, int width, int height)
        {
            MapSection = mapSection;
            Raster = raster;
            Width = width;
            Height = height;
            
            X = new Transformator(mapSection.Left, mapSection.Right, 0, width);
            Y = new Transformator(mapSection.Top, mapSection.Bottom, 0, height);

            System.Threading.ThreadPool.QueueUserWorkItem(weightWorker);
        }

        private void weightWorker(object nothing)
        {
            MapUnit[] stillUnits = MapSection.StillUnits;

            // TODO:
            // Überlegen wie man am besten die Tiles findet die von einer Unit überlagert werden
        }
    }
}
