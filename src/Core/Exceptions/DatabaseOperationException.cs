namespace Core.Exceptions;

public class DatabaseOperationException : Exception
{
    public DatabaseOperationException() : base() { }

    public DatabaseOperationException(string msg) : base(msg) { }

    public DatabaseOperationException(string message, Exception innerException) : base(message, innerException) { }
}