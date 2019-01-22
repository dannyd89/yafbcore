using System;
using System.Collections.Generic;
using System.Text;
using YAFBCore.Mapping;

namespace YAFBCore.Pathfinding.AStarPathing
{
    public sealed class AStarPathfinder
    {
        private readonly MapSectionRaster[] rasters;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rasters"></param>
        internal AStarPathfinder(MapSectionRaster[] rasters)
        {
            this.rasters = rasters;
        }
    }
}
