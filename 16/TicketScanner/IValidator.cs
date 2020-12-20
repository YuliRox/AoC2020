namespace TicketScanner
{
    public interface IValidator<T>
    {
        bool isValid(T thingiToValidate);
    }
}