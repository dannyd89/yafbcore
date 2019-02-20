using System;
using System.Collections.Generic;
using System.Text;

namespace YAFBCore.Mapping
{
    internal class MapSectionRasterConnectingTile
    {
        public readonly MapSectionRasterTile From;
        public MapSectionRasterTile To;

        public bool IsConnecting => (From.Status & MapSectionRasterTileStatus.Blocked) != MapSectionRasterTileStatus.Blocked 
                                    && To != null 
                                    && (To.Status & MapSectionRasterTileStatus.Blocked) != MapSectionRasterTileStatus.Blocked;

        public MapSectionRasterConnectingTile(MapSectionRasterTile from)
        {
            From = from;
        }
    }
}
