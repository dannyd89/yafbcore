using FlattiBase.Ships;
using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Commands
{
    public class MoveCommand : Command
    {
        public readonly Vector Position;

        public MoveCommand(ShipBase owner, Vector position)
            : base(owner)
        {
            Position = position;
        }
    }
}
