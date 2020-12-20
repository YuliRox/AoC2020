using System.Collections.Generic;

namespace TicketScanner
{

    public readonly struct Parameters
    {

        public Parameters(in NamedRange[] namedRanges, in Ticket myTicket, in Ticket[] tickets)
        {
            NamedRanges = namedRanges;
            MyTicket = myTicket;
            Tickets = tickets;
        }

        public readonly NamedRange[] NamedRanges;
        public readonly Ticket MyTicket;
        public readonly Ticket[] Tickets;
    }
}