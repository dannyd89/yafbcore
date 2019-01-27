using Flattiverse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using YAFBCore.Mapping.Units;
using YAFBCore.Utils;
using YAFBCore.Utils.Mathematics;

namespace YAFBCore.Mapping
{
    internal sealed class MapSectionRaster
    {
        internal readonly MapSection MapSection;
        internal readonly MapSectionRasterTile[] Tiles;
        internal readonly int TileSize;
        internal readonly int Width;
        internal readonly int Height;

        //private readonly Transformator X;
        //private readonly Transformator Y;

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
            
            //X = new Transformator(mapSection.Left, mapSection.Right, 0, width);
            //Y = new Transformator(mapSection.Top, mapSection.Bottom, 0, height);

            //System.Threading.ThreadPool.QueueUserWorkItem(weightWorker);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapSection"></param>
        /// <param name="tileSize"></param>
        /// <returns></returns>
        public static MapSectionRaster Rasterize(MapSection mapSection, int tileSize)
        {
            Stopwatch sw = Stopwatch.StartNew();

            int width = (int)(Math.Abs(mapSection.Left) + mapSection.Right + 0.5f);
            int height = (int)(Math.Abs(mapSection.Top) + mapSection.Bottom + 0.5f);
            int mapRasterWidth = (int)((float)width / (float)tileSize + 0.5f);
            int mapRasterHeight = (int)((float)height / (float)tileSize + 0.5f);

            int startX = (int)(mapSection.Left + 0.5f);
            int startY = (int)(mapSection.Top + 0.5f);

            MapSectionRasterTile[] mapRasterTiles = new MapSectionRasterTile[mapRasterWidth * mapRasterHeight];

            MapUnit[] stillUnits = mapSection.StillUnits;
            for (int i = 0; i < mapRasterTiles.Length; i++)
            {
                var tile = new MapSectionRasterTile();
                tile.X = startX + tileSize * (i % mapRasterWidth);
                tile.Y = startY + tileSize * (i / mapRasterWidth);

                for (int unitIndex = 0; unitIndex < stillUnits.Length; unitIndex++)
                {
                    MapUnit mapUnit = stillUnits[unitIndex];

                    if (mapUnit == null)
                        break;

                    // TODO: Hier sollte noch eine Überprüfung für spezielle Units rein
                    // die je nachdem einen Bereich als schlecht oder gut definieren
                    // Sonne z.B. die Koronas als gut, Blackholes der Wirkungskreis als schlecht

                    if (mapUnit.IsSolid 
                        && mapUnit.Mobility == Mobility.Still 
                        && intersects(tile, tileSize, mapUnit.PositionInternal, mapUnit.RadiusInternal))
                        tile.Status = MapSectionRasterTileStatus.Blocked;
                }

                mapRasterTiles[i] = tile;
            }

            Console.WriteLine("Raster time: " + sw.Elapsed);
            
            return new MapSectionRaster(mapSection, mapRasterTiles, tileSize, mapRasterWidth, mapRasterHeight);
        }

        /// <summary>
        /// Checks if unit intersects with the given tile
        /// </summary>
        /// <param name="tile">Tile to use for check</param>
        /// <param name="tileSize">Size of the tile</param>
        /// <param name="position">Position of the unit</param>
        /// <param name="radius">Radius to use for the check, useful for checking if coronas intersect with the tile</param>
        /// <returns>True if intersecting with tile</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool intersects(MapSectionRasterTile tile, int tileSize, Vector position, float radius)
        {
            float x = position.X, y = position.Y;

            // Find the closest point to the circle within the rectangle
            //float closestX = MathUtil.Clamp(x, tile.X, tile.X + tileSize);
            //float closestY = MathUtil.Clamp(y, tile.Y, tile.Y + tileSize);

            // Calculate the distance between the circle's center and this closest point
            float distanceX = x - MathUtil.Clamp(x, tile.X, tile.X + tileSize);
            float distanceY = y - MathUtil.Clamp(y, tile.Y, tile.Y + tileSize);

            // If the distance is less than the circle's radius, an intersection occurs
            return (distanceX * distanceX) + (distanceY * distanceY) < (radius * radius);
        }
    }
}
