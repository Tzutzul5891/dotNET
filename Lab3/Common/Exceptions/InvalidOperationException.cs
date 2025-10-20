namespace BookApi.Common.Exceptions;

public class InvalidBookOperationException : Exception
{
    public InvalidBookOperationException(string message) : base(message)
    {
    }

    public InvalidBookOperationException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }

    public InvalidBookOperationException(string message, string operationType) 
        : base(message)
    {
        OperationType = operationType;
    }

    public string? OperationType { get; }
}
