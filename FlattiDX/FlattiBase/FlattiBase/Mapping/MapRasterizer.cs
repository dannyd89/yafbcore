using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Mapping
{
    internal class MapRasterizer
    {
        public readonly Map Map;

        /// <summary>
        /// Creates a map rasterizer
        /// </summary>
        /// <param name="map">Map to rasterize</param>
        public MapRasterizer(Map map)
        {
            Map = map;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tileSize"></param>
        /// <returns></returns>
        public MapRaster RasterizeMap(int tileSize)
        {
            int width = (int)(Math.Abs(Map.Left) + Map.Right + 0.5f);
            int height = (int)(Math.Abs(Map.Top) + Map.Bottom + 0.5f);
            int mapRasterWidth = (int)((float)width / (float)tileSize + 0.5f);
            int mapRasterHeight = (int)((float)height / (float)tileSize + 0.5f);

            int startX = (int)(Map.Left + 0.5f);
            int startY = (int)(Map.Top + 0.5f);

            MapRasterTile[] mapRasterTiles = new MapRasterTile[mapRasterWidth * mapRasterHeight];

            for (int i = 0; i < mapRasterTiles.Length; i++)
            {
                MapRasterTile tile = new MapRasterTile();
                tile.X = startX + tileSize * (i % mapRasterWidth);
                tile.Y = startY + tileSize * (i / mapRasterWidth);
                tile.Size = tileSize;

                mapRasterTiles[i] = tile;
            }

            return new MapRaster(Map, mapRasterTiles, new System.Drawing.Size(width, height));
        }
    }
}
