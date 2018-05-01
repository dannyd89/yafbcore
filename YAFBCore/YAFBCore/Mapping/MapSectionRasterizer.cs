using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAFBCore.Mapping
{
    internal static class MapSectionRasterizer
    {
        /// <summary>
        /// 
        /// </summary>
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
                MapSectionRasterTile tile = new MapSectionRasterTile();
                tile.X = startX + tileSize * (i % mapRasterWidth);
                tile.Y = startY + tileSize * (i / mapRasterWidth);
                tile.Size = tileSize;

                mapRasterTiles[i] = tile;
            }

            return new MapSectionRaster(mapSection, mapRasterTiles, tileSize, mapRasterWidth, mapRasterHeight);
        }
    }
}
