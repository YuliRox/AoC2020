using System.Linq;

namespace TicketScanner
{

    public class TicketValidator : IValidator<Ticket>
    {
        private readonly IValidator<int> valueValidator;

        public TicketValidator(IValidator<int> valueValidator)
        {
            this.valueValidator = valueValidator;
        }

        public bool isValid(Ticket ticket)
        {
            return ticket.TicketValues.Count(ticketValue => !valueValidator.isValid(ticketValue)) == 0;
        }
    }
}