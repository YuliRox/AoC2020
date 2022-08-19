namespace TicketScanner
{
    public readonly struct Ticket
    {
        public Ticket(int[] ticketValues)
        {
            TicketValues = ticketValues;
        }
        public int[] TicketValues { get; }
    }

}