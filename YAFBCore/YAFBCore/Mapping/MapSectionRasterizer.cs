using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAFBCore.Mapping
{
    internal class MapSectionRasterizer
    {
        public readonly MapSection MapSection;

        /// <summary>
        /// Creates a map rasterizer
        /// </summary>
        /// <param name="mapSection">Map to rasterize</param>
        public MapSectionRasterizer(MapSection mapSection)
        {
            MapSection = mapSection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tileSize"></param>
        /// <returns></returns>
        public MapSectionRaster Rasterize(int tileSize)
        {
            int width = (int)(Math.Abs(MapSection.Left) + MapSection.Right + 0.5f);
            int height = (int)(Math.Abs(MapSection.Top) + MapSection.Bottom + 0.5f);
            int mapRasterWidth = (int)((float)width / (float)tileSize + 0.5f);
            int mapRasterHeight = (int)((float)height / (float)tileSize + 0.5f);

            int startX = (int)(MapSection.Left + 0.5f);
            int startY = (int)(MapSection.Top + 0.5f);

            MapSectionRasterTile[] mapRasterTiles = new MapSectionRasterTile[mapRasterWidth * mapRasterHeight];

            for (int i = 0; i < mapRasterTiles.Length; i++)
            {
                MapSectionRasterTile tile = new MapSectionRasterTile();
                tile.X = startX + tileSize * (i % mapRasterWidth);
                tile.Y = startY + tileSize * (i / mapRasterWidth);
                tile.Size = tileSize;

                mapRasterTiles[i] = tile;
            }

            return new MapSectionRaster(MapSection, mapRasterTiles, new System.Drawing.Size(width, height));
        }
    }
}
