using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAFBCore.Mapping
{
    internal class MapSectionRasterTile
    {
        public float X;
        public float Y;
        public int Size;
        public byte Weight;
        public ushort Value;
        public MapSectionRasterTile LastTile;

        public MapSectionRasterTile()
        {
            X = 0f;
            Y = 0f;
            Size = 0;
            Weight = 1;
            Value = 0;
            LastTile = null;
        }
    }
}
