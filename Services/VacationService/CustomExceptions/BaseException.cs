namespace VacationService.CustomExceptions;

public class BaseException : Exception
{
    public virtual int StatusCode { get; }
    public virtual string Header { get; }

    public BaseException() { }

    public BaseException(string message) : base(message) { }

    public BaseException(string message, Exception innerException) : base(message, innerException) { }
}