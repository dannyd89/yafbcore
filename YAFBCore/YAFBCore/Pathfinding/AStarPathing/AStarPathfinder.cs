using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YAFBCore.Mapping;

namespace YAFBCore.Pathfinding.AStarPathing
{
    public sealed class AStarPathfinder
    {
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
        /// 
        /// </summary>
        public bool IsReady { get; private set; }

        /// <summary>
        /// Creates a 
        /// </summary>
        /// <param name="rasters"></param>
        internal AStarPathfinder(int tileSize)
        {
            TileSize = tileSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapSections"></param>
        internal void UpdateRasterAsync(MapSection[] mapSections)
        {
            waitEvent.Reset();
            IsReady = false;

            ThreadPool.QueueUserWorkItem(update, mapSections);
        }

        /// <summary>
        /// Waits until the path finder is ready to be used
        /// </summary>
        public void WaitReady()
        {
            waitEvent.WaitOne();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void update(object arg)
        {
            MapSection[] mapSections = (MapSection[])arg;

            Task<MapSectionRaster>[] tasks = new Task<MapSectionRaster>[mapSections.Length];
            for (int i = 0; i < mapSections.Length; i++)
                tasks[i] = mapSections[i].GetRaster(TileSize);

            Task.WaitAll(tasks);

            // TODO: I dont think that we need to do this if we have the same amount of rasters
            if (rasters == null || rasters.Length != mapSections.Length)
            {
                rasters = new MapSectionRaster[mapSections.Length];
                for (int i = 0; i < rasters.Length; i++)
                    rasters[i] = tasks[i].Result;
            }

            waitEvent.Set();
        }
    }
}
