namespace TicketScanner
{
    public class AggregatingTicketValueValidator : IValidator<int>
    {
        private readonly IValidator<int> inner;

        public AggregatingTicketValueValidator(IValidator<int> inner)
        {
            this.inner = inner;
        }

        public int ErrorRate { get; private set; }

        public bool isValid(int thingiToValidate)
        {
            if (inner.isValid(thingiToValidate))
                return true;

            ErrorRate += thingiToValidate;
            return false;
        }

        public void Reset()
        {
            ErrorRate = 0;
        }
    }
}