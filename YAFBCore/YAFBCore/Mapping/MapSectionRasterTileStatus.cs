using System;
using System.Collections.Generic;
using System.Text;

namespace YAFBCore.Mapping
{
    internal enum MapSectionRasterTileStatus : byte
    {
        Free = 0b0000_0000, // Tile is free to traverse
        Checked = 0b0000_0001, // Tile was checked by path finder
        Connecting = 0b0000_1000, // Tile is connecting one section with another
        Blocked = 0b1000_0000 // Tile is blocked by a solid unit
    }
}
