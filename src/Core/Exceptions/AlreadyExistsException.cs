namespace Core.Exceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException()
    {
    }

    public AlreadyExistsException(string msg) : base(msg)
    {
    }

    public AlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}