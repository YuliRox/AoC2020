using System.Linq;

namespace TicketScanner
{
    public class AggregatingTicketValidator : IValidator<Ticket>
    {

        public AggregatingTicketValidator(IValidator<Ticket> ticketValidator, IValidator<int> valueValidator)
        {
            this.ticketValidator = ticketValidator;
            this.valueValidator = valueValidator;
        }

        private readonly IValidator<Ticket> ticketValidator;
        private readonly IValidator<int> valueValidator;

        public int TicketScanningErrorRate { get; set; } = 0;

        public bool isValid(Ticket thingiToValidate)
        {
            if (ticketValidator.isValid(thingiToValidate))
                return true;

            TicketScanningErrorRate += AggregateInvalidValues(thingiToValidate);
            return false;
        }

        private int AggregateInvalidValues(Ticket ticket)
        {
            return ticket.TicketValues.Where(ticketValue => !valueValidator.isValid(ticketValue)).Sum();
        }
    }
}