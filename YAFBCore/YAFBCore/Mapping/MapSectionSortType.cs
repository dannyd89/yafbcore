using System;
using System.Collections.Generic;
using System.Text;

namespace YAFBCore.Mapping
{
    internal enum MapSectionSortType : short
    {
        None = 0x0000,
        StillUnits = 0x0001,
        AgingUnits = 0x0010,
        PlayerUnits = 0x0100,
    }
}
