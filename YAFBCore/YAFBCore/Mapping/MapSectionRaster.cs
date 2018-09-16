using Flattiverse;
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

            foreach (MapUnit mapUnit in stillUnits)
            {
                // TODO:
                // Hier ist noch weiteres Optimierungspotential
                // Nur Tiles ansehen die auch im Bereich der Unit liegen
                foreach (MapSectionRasterTile tile in Raster)
                    if (intersects(mapUnit, tile))
                        tile.Weight = 255;
            }
        }

        /// <summary>
        /// Checks if unit intersects with the given tile
        /// </summary>
        /// <param name="mapUnit"></param>
        /// <param name="tile"></param>
        /// <returns>True if intersects with tile</returns>
        private static bool intersects(MapUnit mapUnit, MapSectionRasterTile tile)
        {
            float x = mapUnit.PositionInternal.X, y = mapUnit.PositionInternal.Y;

            // Find the closest point to the circle within the rectangle
            float closestX = MathUtil.Clamp(x, tile.X, tile.X + tile.Size);
            float closestY = MathUtil.Clamp(y, tile.Y, tile.Y + tile.Size);

            // Calculate the distance between the circle's center and this closest point
            float distanceX = x - closestX;
            float distanceY = y - closestY;

            // If the distance is less than the circle's radius, an intersection occurs
            float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
            return distanceSquared < (mapUnit.Radius * mapUnit.Radius);
        }
    }
}
