using System.Collections.Generic;
using System.Linq;

namespace TicketScanner
{
    public class TicketValueValidator : IValidator<int>
    {
        private readonly NamedRange invalidRanges;

        public TicketValueValidator(NamedRange invalidRanges)
        {
            this.invalidRanges = invalidRanges;
        }

        public static NamedRange CreateInvalidRanges(IEnumerable<Range> ranges)
        {
            var invalidRanges = new List<Range>();

            var currentBound = 0;

            var sortedRanges = ranges
                .OrderBy(x => x.Start)
                .ThenByDescending(x => x.End)
                .ToArray();

            foreach (var range in sortedRanges)
            {
                if (currentBound < range.Start)
                {
                    invalidRanges.Add(new Range(currentBound, range.Start - 1));
                    currentBound = range.End + 1;
                }
                if (currentBound >= range.Start && currentBound <= range.End)
                {
                    currentBound = range.End + 1;
                }
            }

            invalidRanges.Add(new Range(currentBound, int.MaxValue));
            var invalidRangesAsArray = invalidRanges.ToArray();

            return new NamedRange("Invalid", ref invalidRangesAsArray);
        }

        public bool isValid(int thingiToValidate)
        {
            return !invalidRanges.InRange(thingiToValidate);
        }
    }
}