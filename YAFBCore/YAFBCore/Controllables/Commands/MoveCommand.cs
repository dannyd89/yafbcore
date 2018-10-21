using System;
using System.Collections.Generic;
using System.Text;

namespace YAFBCore.Controllables.Commands
{
    public class MoveCommand : Command
    {
        public readonly float X;
        public readonly float Y;
        
        public bool Reached;

        public override CommandType Type => CommandType.Move;

        public MoveCommand(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
