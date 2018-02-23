using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Helper
{
    public class LogEntry : IComparable<LogEntry>
    {
        public readonly string Name;
        public readonly TimeSpan Elapsed;

        public LogEntry(string name, TimeSpan elapsed)
        {
            Name = name;
            Elapsed = elapsed;
        }

        public int CompareTo(LogEntry other)
        {
            if (Name != other.Name)
                return string.Compare(Name, other.Name);

            if (Elapsed < other.Elapsed)
                return 1;
            else if (Elapsed > other.Elapsed)
                return -1;

            return 0;
        }
    }
}
