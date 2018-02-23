using FlattiBase.Ships;
using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlattiBase.Commands
{
    public abstract class Command
    {
        public readonly ShipBase Owner;

        public Command(ShipBase owner)
        {
 
        }
    }
}
