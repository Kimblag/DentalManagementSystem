namespace DentalSystem.Application.Exceptions
{
    public class ApplicationNotFoundException(string message) : ApplicationException(message)
    {
    }
}
