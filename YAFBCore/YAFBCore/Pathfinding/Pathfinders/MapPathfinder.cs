using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YAFBCore.Mapping;
using YAFBCore.Utils;

namespace YAFBCore.Pathfinding.Pathfinders
{
    public sealed class MapPathfinder
    {
        /// <summary>
        /// Map this pathfinder is running on
        /// </summary>
        internal readonly Map Map;

        /// <summary>
        /// Tile size this path finder is based on
        /// </summary>
        internal readonly int TileSize;

        /// <summary>
        /// 
        /// </summary>
        private MapSectionRaster[] rasters;

        /// <summary>
        /// Enables to wait for the pathfinder to be ready
        /// </summary>
        private ManualResetEvent waitEvent = new ManualResetEvent(true);

        /// <summary>
        /// Creates a 
        /// </summary>
        /// <param name="rasters"></param>
        internal MapPathfinder(int tileSize, Map map)
        {
            TileSize = tileSize;
            Map = map;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapSections"></param>
        /// <param name="sectionCount">Count of sections into 1 dimension, mapSections is sectionCount * sectionCount big</param>
        internal void UpdateRasterAsync(MapSection[] mapSections, int sectionCount)
        {
            ThreadPool.QueueUserWorkItem(update, new object[] { mapSections, sectionCount });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void update(object arg)
        {
            Stopwatch sw = Stopwatch.StartNew();

            waitEvent.WaitOne();
            waitEvent.Reset();

            object[] args = (object[])arg;

            MapSection[] mapSections = (MapSection[])args[0];
            int sectionCount = (int)args[1];

            MapSectionRaster[] tempRasters = new MapSectionRaster[mapSections.Length];

            Task<MapSectionRaster>[] tasks = new Task<MapSectionRaster>[mapSections.Length];
            for (int i = 0; i < mapSections.Length; i++)
                tasks[i] = mapSections[i].GetRaster(TileSize);

            Task.WaitAll(tasks);

            tempRasters = new MapSectionRaster[mapSections.Length];
            for (int i = 0; i < tempRasters.Length; i++)
                tempRasters[i] = tasks[i].Result;

            for (int i = 0; i < tempRasters.Length; i++)
            {
                int x = i % sectionCount, y = i / sectionCount;

                if (tempRasters[i].TopConnectingTiles[0].To == null && tryGetMapSectionIndex(sectionCount, x, y - 1, out int topIndex))
                {
                    MapSectionRasterConnectingTile[] bottomTiles = tempRasters[topIndex].BottomConnectingTiles;

                    for (int tempIndex = 0; tempIndex < bottomTiles.Length; tempIndex++)
                    {
                        tempRasters[i].TopConnectingTiles[tempIndex].To = bottomTiles[tempIndex].From;
                        bottomTiles[tempIndex].To = tempRasters[i].TopConnectingTiles[tempIndex].From;
                    }
                }

                if (tempRasters[i].RightConnectingTiles[0].To == null && tryGetMapSectionIndex(sectionCount, x + 1, y, out int rightIndex))
                {
                    MapSectionRasterConnectingTile[] leftTiles = tempRasters[rightIndex].LeftConnectingTiles;

                    for (int tempIndex = 0; tempIndex < leftTiles.Length; tempIndex++)
                    {
                        tempRasters[i].RightConnectingTiles[tempIndex].To = leftTiles[tempIndex].From;
                        leftTiles[tempIndex].To = tempRasters[i].RightConnectingTiles[tempIndex].From;
                    }
                }

                if (tempRasters[i].BottomConnectingTiles[0].To == null && tryGetMapSectionIndex(sectionCount, x, y + 1, out int bottomIndex))
                {
                    MapSectionRasterConnectingTile[] topTiles = tempRasters[bottomIndex].TopConnectingTiles;

                    for (int tempIndex = 0; tempIndex < topTiles.Length; tempIndex++)
                    {
                        tempRasters[i].BottomConnectingTiles[tempIndex].To = topTiles[tempIndex].From;
                        topTiles[tempIndex].To = tempRasters[i].BottomConnectingTiles[tempIndex].From;
                    }
                }

                if (tempRasters[i].LeftConnectingTiles[0].To == null && tryGetMapSectionIndex(sectionCount, x - 1, y, out int leftIndex))
                {
                    MapSectionRasterConnectingTile[] rightTiles = tempRasters[leftIndex].RightConnectingTiles;

                    for (int tempIndex = 0; tempIndex < rightTiles.Length; tempIndex++)
                    {
                        tempRasters[i].LeftConnectingTiles[tempIndex].To = rightTiles[tempIndex].From;
                        rightTiles[tempIndex].To = tempRasters[i].LeftConnectingTiles[tempIndex].From;
                    }
                }
            }

            waitEvent.Set();

            Console.WriteLine("Pathfinder update time: " + sw.Elapsed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool tryGetMapSectionIndex(int sectionCount, int x, int y, out int index)
        {
            index = -1;

            if (x < 0 || x >= sectionCount || y < 0 || y >= sectionCount)
                return false;

            index = x + y * sectionCount;
            return true;
        }
    }
}
