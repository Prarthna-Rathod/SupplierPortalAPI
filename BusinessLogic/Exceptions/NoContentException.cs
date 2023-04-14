namespace SupplierPortalAPI.Infrastructure.Middleware.Exceptions;

public class NoContentException : Exception
{
    public NoContentException(string message) : base(message) { }
}
