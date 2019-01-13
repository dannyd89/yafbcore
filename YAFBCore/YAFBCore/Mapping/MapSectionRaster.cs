using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        internal readonly MapSectionRasterTile[] Tiles;
        internal readonly int TileSize;
        internal readonly int Width;
        internal readonly int Height;

        private readonly Transformator X;
        private readonly Transformator Y;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapSection"></param>
        /// <param name="tiles"></param>
        /// <param name="size"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private MapSectionRaster(MapSection mapSection, MapSectionRasterTile[] tiles, int size, int width, int height)
        {
            MapSection = mapSection;
            Tiles = tiles;
            TileSize = size;
            Width = width;
            Height = height;
            
            X = new Transformator(mapSection.Left, mapSection.Right, 0, width);
            Y = new Transformator(mapSection.Top, mapSection.Bottom, 0, height);

            System.Threading.ThreadPool.QueueUserWorkItem(weightWorker);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapSection"></param>
        /// <param name="tileSize"></param>
        /// <returns></returns>
        public static MapSectionRaster Rasterize(MapSection mapSection, int tileSize)
        {
            int width = (int)(Math.Abs(mapSection.Left) + mapSection.Right + 0.5f);
            int height = (int)(Math.Abs(mapSection.Top) + mapSection.Bottom + 0.5f);
            int mapRasterWidth = (int)((float)width / (float)tileSize + 0.5f);
            int mapRasterHeight = (int)((float)height / (float)tileSize + 0.5f);

            int startX = (int)(mapSection.Left + 0.5f);
            int startY = (int)(mapSection.Top + 0.5f);

            MapSectionRasterTile[] mapRasterTiles = new MapSectionRasterTile[mapRasterWidth * mapRasterHeight];

            for (int i = 0; i < mapRasterTiles.Length; i++)
            {
                var tile = new MapSectionRasterTile();
                tile.X = startX + tileSize * (i % mapRasterWidth);
                tile.Y = startY + tileSize * (i / mapRasterWidth);

                mapRasterTiles[i] = new MapSectionRasterTile();
            }
            
            return new MapSectionRaster(mapSection, mapRasterTiles, tileSize, mapRasterWidth, mapRasterHeight);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nothing"></param>
        private void weightWorker(object nothing)
        {
            MapUnit[] stillUnits = MapSection.StillUnits;

            for (int unitIndex = 0; unitIndex < stillUnits.Length; unitIndex++)
            {
                MapUnit mapUnit = stillUnits[unitIndex];

                if (mapUnit == null)
                    break;

                for (int rasterIndex = 0; rasterIndex < Tiles.Length; rasterIndex++)
                    if (intersects(mapUnit, Tiles[rasterIndex]))
                    {
                        // TODO: Hier sollte noch eine Überprüfung für spezielle Units rein
                        // die je nachdem einen Bereich als schlecht oder gut definieren
                        // Sonne z.B. die Koronas als gut, Blackholes der Wirkungskreis als schlecht

                        
                        //tile.Weight = 255;
                    }
            }
        }

        /// <summary>
        /// Checks if unit intersects with the given tile
        /// </summary>
        /// <param name="mapUnit"></param>
        /// <param name="tile"></param>
        /// <returns>True if intersects with tile</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool intersects(MapUnit mapUnit, MapSectionRasterTile tile)
        {
            float x = mapUnit.PositionInternal.X, y = mapUnit.PositionInternal.Y;

            // Find the closest point to the circle within the rectangle
            float closestX = MathUtil.Clamp(x, tile.X, tile.X + TileSize);
            float closestY = MathUtil.Clamp(y, tile.Y, tile.Y + TileSize);

            // Calculate the distance between the circle's center and this closest point
            float distanceX = x - closestX;
            float distanceY = y - closestY;

            // If the distance is less than the circle's radius, an intersection occurs
            float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
            return distanceSquared < (mapUnit.Radius * mapUnit.Radius);
        }
    }
}
