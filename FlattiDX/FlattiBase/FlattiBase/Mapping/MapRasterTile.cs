using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Mapping
{
    internal class MapRasterTile
    {
        public float X;
        public float Y;
        public int Size;
        public int Weight;
        public int Value;
        public MapRasterTile LastTile;

        public MapRasterTile()
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
