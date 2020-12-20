using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TicketScanner
{
    public class InputReader
    {
        private Regex namedRange = new Regex(@"^([\w\s]+):\s*(?:\d+-\d+\s*(?:or)?\s*)+$");
        private Regex singleRangePattern = new Regex(@"(\d+)-(\d+)");
        private Regex ticketPattern = new Regex(@"^[\d+,]+$");


        public Parameters ReadConfig(string pathToInputFile)
        {
            using var textStream = new StreamReader(pathToInputFile);

            var namedRanges = ReadFieldRanges(textStream);
            var myTicket = ReadTicket(textStream);
            var tickets = ReadTickets(textStream);

            if (!myTicket.HasValue){
                throw new Exception("Personal Ticket not found during scanning");
            }

            Ticket definitivMyTicket = myTicket.Value;

            return new Parameters(namedRanges, in definitivMyTicket, tickets);
        }

        private NamedRange[] ReadFieldRanges(StreamReader textStream)
        {
            var namedRanges = new List<NamedRange>();

            string line;

            while (!String.IsNullOrWhiteSpace(line = textStream.ReadLine()))
            {
                if (!namedRange.IsMatch(line))
                    continue;

                namedRanges.Add(ParseNamedGroup(line));
            }

            return namedRanges.ToArray();
        }

        private NamedRange ParseNamedGroup(string line)
        {
            var pieces = namedRange.Match(line);
            var matchedName = pieces.Groups[1].Value;

            var rangeMatches = singleRangePattern.Matches(line);
            var ranges = new List<Range>();

            foreach (var match in rangeMatches.OfType<Match>())
            {
                var start = Int32.Parse(match.Groups[1].Value);
                var end = Int32.Parse(match.Groups[2].Value);
                ranges.Add(new Range(start, end));
            }
            
            var finalRanges = ranges.ToArray();

            return new NamedRange(matchedName, ref finalRanges);
        }

        private int[] ParseTicketValues(string line)
        {
            return ticketPattern
                .Match(line)
                .Value
                .Split(',')
                .Select(int.Parse)
                .ToArray();
        }

        private Ticket? ReadTicket(StreamReader textStream)
        {
            string line;
            while ((line = textStream.ReadLine()) != null)
            {
                if (ticketPattern.IsMatch(line))
                {
                    return new Ticket(ParseTicketValues(line));
                }
            }
            return null;
        }

        private Ticket[] ReadTickets(StreamReader textStream)
        {
            var tickets = new List<Ticket>();
            Ticket? newTicket;

            while ((newTicket = ReadTicket(textStream)) != null)
            {
                tickets.Add(newTicket.Value);
            }

            return tickets.ToArray();
        }
    }
}