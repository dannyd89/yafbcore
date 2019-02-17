using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YAFBCore.Mapping;

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
        private ManualResetEvent waitEvent = new ManualResetEvent(false);

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
        internal void UpdateRasterAsync(MapSection[] mapSections)
        {
            ThreadPool.QueueUserWorkItem(update, mapSections);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void update(object arg)
        {
            waitEvent.WaitOne();
            waitEvent.Reset();

            MapSection[] mapSections = (MapSection[])arg;
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

            }

            waitEvent.Set();
        }
    }
}
