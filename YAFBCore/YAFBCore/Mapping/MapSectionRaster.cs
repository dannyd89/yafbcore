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
        internal readonly List<MapSectionRasterTile> ConnectingTiles;
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
        private MapSectionRaster(MapSection mapSection, MapSectionRasterTile[] tiles, List<MapSectionRasterTile> connectingTiles, int size, int width, int height)
        {
            MapSection = mapSection;
            Tiles = tiles;
            ConnectingTiles = connectingTiles;
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

            float width = Math.Abs(mapSection.Left) + mapSection.Right;
            float height = Math.Abs(mapSection.Top) + mapSection.Bottom;
            int mapRasterWidth = (int)(width / tileSize + 0.5f);
            int mapRasterHeight = (int)(height / tileSize + 0.5f);

            int startX = (int)(mapSection.Left + 0.5f);
            int startY = (int)(mapSection.Top + 0.5f);

            MapSectionRasterTile[] mapRasterTiles = new MapSectionRasterTile[mapRasterWidth * mapRasterHeight];
            List<MapSectionRasterTile> connectingTiles = new List<MapSectionRasterTile>();

            MapUnit[] stillUnits = mapSection.StillUnits;
            for (int i = 0; i < mapRasterTiles.Length; i++)
            {
                int xIndex = i % mapRasterWidth, yIndex = i / mapRasterWidth;

                var tile = new MapSectionRasterTile();
                tile.X = startX + tileSize * xIndex;
                tile.Y = startY + tileSize * yIndex;

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

                if (tile.Status != MapSectionRasterTileStatus.Blocked
                    && (xIndex == 0 || yIndex == 0))
                {
                    tile.Status |= MapSectionRasterTileStatus.Connecting;
                    connectingTiles.Add(tile);
                }

                mapRasterTiles[i] = tile;
            }

            Console.WriteLine("Raster time: " + sw.Elapsed);
            
            return new MapSectionRaster(mapSection, mapRasterTiles, connectingTiles, tileSize, mapRasterWidth, mapRasterHeight);
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
            // Find the closest point to the circle within the rectangle
            // Calculate the distance between the circle's center and this closest point
            float distanceX = position.X - MathUtil.Clamp(position.X, tile.X, tile.X + tileSize);
            float distanceY = position.Y - MathUtil.Clamp(position.Y, tile.Y, tile.Y + tileSize);

            // If the distance is less than the circle's radius, an intersection occurs
            return (distanceX * distanceX) + (distanceY * distanceY) < (radius * radius);
        }
    }
}
