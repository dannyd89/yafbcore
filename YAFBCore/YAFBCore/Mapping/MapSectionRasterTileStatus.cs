using System;
using System.Collections.Generic;
using System.Text;

namespace YAFBCore.Mapping
{
    internal enum MapSectionRasterTileStatus : byte
    {
        Free = 0b0000_0000, // Tile is free to traverse
        Checked = 0b0000_0001, // Tile was checked by path finder
        Start = 0b0000_0010, // Path finding started with this tile
        Finish = 0b0000_0100, // Path finding finishes at this tile
        Connecting = 0b0000_1000, // Tile is connecting one section with another
        Blocked = 0b1000_0000 // Tile is blocked by a solid unit
    }
}
