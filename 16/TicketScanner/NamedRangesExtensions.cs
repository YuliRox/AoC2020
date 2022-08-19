namespace TicketScanner
{
    public static class NamedRangesExtensions
    {
        public static bool InRange(in this NamedRange namedRange, int test)
        {
            foreach (var range in namedRange.Ranges)
            {
                if (range.Start <= test && range.End >= test)
                    return true;
            }
            return false;
        }

        public static bool FastRange(in this NamedRange namedRange, int test)
        {
            return
                namedRange.Ranges[0].Start >= test && namedRange.Ranges[0].End <= test ||
                namedRange.Ranges[1].Start >= test && namedRange.Ranges[1].End <= test;
        }
    }
}