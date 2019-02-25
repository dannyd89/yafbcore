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
    public sealed class MapSectionRaster
    {
        internal readonly MapSection MapSection;
        public readonly MapSectionRasterTile[] Tiles;

        internal readonly MapSectionRasterConnectingTile[] TopConnectingTiles;
        internal readonly MapSectionRasterConnectingTile[] RightConnectingTiles;
        internal readonly MapSectionRasterConnectingTile[] BottomConnectingTiles;
        internal readonly MapSectionRasterConnectingTile[] LeftConnectingTiles;

        internal readonly int Size;
        public readonly int TileSize;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapSection"></param>
        /// <param name="tileSize"></param>
        internal MapSectionRaster(MapSection mapSection, int tileSize)
        {
            MapSection = mapSection;
            TileSize = tileSize;

            //Stopwatch sw = Stopwatch.StartNew();

            //float width = mapSection.Left < 0 ? Math.Abs(mapSection.Left) + mapSection.Right : Math.Abs(mapSection.Left - mapSection.Right);
            //float height = mapSection.Top < 0 ? Math.Abs(mapSection.Top) + mapSection.Bottom : Math.Abs(mapSection.Top - mapSection.Bottom); 
            Size = (int)((float)Map.SectionSize / tileSize + 0.5f);

            int startX = (int)(mapSection.Left + 0.5f);
            int startY = (int)(mapSection.Top + 0.5f);

            Tiles = new MapSectionRasterTile[Size * Size];

            TopConnectingTiles = new MapSectionRasterConnectingTile[Size];
            RightConnectingTiles = new MapSectionRasterConnectingTile[Size];
            BottomConnectingTiles = new MapSectionRasterConnectingTile[Size];
            LeftConnectingTiles = new MapSectionRasterConnectingTile[Size];

            MapUnit[] stillUnits = mapSection.StillUnits;
            for (int i = 0; i < Tiles.Length; i++)
            {
                int xIndex = i % Size, yIndex = i / Size;

                var tile = new MapSectionRasterTile();
                tile.X = startX + tileSize * xIndex + tileSize / 2f;
                tile.Y = startY + tileSize * yIndex + tileSize / 2f;

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

                if (xIndex == 0)
                {
                    tile.Status |= MapSectionRasterTileStatus.Connecting;
                    LeftConnectingTiles[yIndex] = new MapSectionRasterConnectingTile(tile);
                }
                else if (xIndex == (Size - 1))
                {
                    tile.Status |= MapSectionRasterTileStatus.Connecting;
                    RightConnectingTiles[yIndex] = new MapSectionRasterConnectingTile(tile);
                }

                if (yIndex == 0)
                {
                    tile.Status |= MapSectionRasterTileStatus.Connecting;
                    TopConnectingTiles[xIndex] = new MapSectionRasterConnectingTile(tile);
                }
                else if (yIndex == (Size - 1))
                {
                    tile.Status |= MapSectionRasterTileStatus.Connecting;
                    BottomConnectingTiles[xIndex] = new MapSectionRasterConnectingTile(tile);
                }

                Tiles[i] = tile;
            }

            //Console.WriteLine("Raster time: " + sw.Elapsed);
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
            float distanceX = position.X - MathUtil.Clamp(position.X, tile.X - tileSize / 2f, tile.X + tileSize / 2f);
            float distanceY = position.Y - MathUtil.Clamp(position.Y, tile.Y - tileSize / 2f, tile.Y + tileSize / 2f);

            // If the distance is less than the circle's radius, an intersection occurs
            return (distanceX * distanceX) + (distanceY * distanceY) < (radius * radius);
        }
    }
}
