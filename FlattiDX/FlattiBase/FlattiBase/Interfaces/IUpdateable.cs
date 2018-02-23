using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Interfaces
{
    internal interface IUpdateable
    {
        void Update(TimeSpan lastTime);
    }
}
