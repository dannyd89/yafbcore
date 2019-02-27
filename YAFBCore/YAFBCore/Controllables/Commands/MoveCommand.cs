using System;
using System.Collections.Generic;
using System.Text;

namespace YAFBCore.Controllables.Commands
{
    public class MoveCommand : Command
    {
        public readonly float X;
        public readonly float Y;

        internal readonly Flattiverse.Vector Position;

        internal bool DontStop;
        internal bool Reached;

        public override CommandType Type => CommandType.Move;

        public MoveCommand(float x, float y)
        {
            X = x;
            Y = y;

            Position = new Flattiverse.Vector(x, y);
        }
    }
}
