using System.Collections.Generic;

namespace TicketScanner
{
    public readonly struct NamedRange
    {
        public string Name { get; }
        public Range[] Ranges { get; }

        public NamedRange(in string name, ref Range[] ranges)
        {
            Name = name;
            Ranges = ranges;
        }
    }
}