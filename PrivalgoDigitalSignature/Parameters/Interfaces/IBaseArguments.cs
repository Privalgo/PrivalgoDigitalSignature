namespace PrivalgoDigitalSignature.Parameters.Interfaces;

/* The code is defining a C# interface called `IBaseArguments`. An interface is a contract that defines
a set of properties, methods, and events that a class must implement. */
public interface IBaseArguments
{
    string Data { get; set; }
    string DataFileName { get; set; }
    string InputData { get; }
}
