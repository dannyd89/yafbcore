using System;
using System.Collections.Generic;
using System.Text;

namespace YAFBCore.Controllables.Commands
{
    public abstract class Command
    {
        public virtual CommandType Type => CommandType.Invalid;
    }
}
