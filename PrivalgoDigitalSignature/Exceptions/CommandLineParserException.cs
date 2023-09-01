namespace PrivalgoDigitalSignature.Exceptions;

/* The `CommandLineParserException` class is a custom exception class that can be used to handle errors
related to command line parsing. */
public class CommandLineParserException : Exception
{
    /* The code `public CommandLineParserException(string message) : base(message)` is a constructor
    for the `CommandLineParserException` class. */
    public CommandLineParserException(string message)
        : base(message)
    {
    }

    /* The code `public CommandLineParserException(string message, Exception innerException) :
    base(message, innerException)` is a constructor for the `CommandLineParserException` class that
    takes two parameters: `message` and `innerException`. */
    public CommandLineParserException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
