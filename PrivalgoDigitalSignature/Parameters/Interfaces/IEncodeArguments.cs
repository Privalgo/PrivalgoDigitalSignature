namespace PrivalgoDigitalSignature.Parameters.Interfaces;

/* The code is defining a C# interface named `IEncodeArguments` that inherits from two other
interfaces: `IBaseArguments` and `IValidatable`. This means that any class that implements the
`IEncodeArguments` interface must also implement the members defined in both `IBaseArguments` and
`IValidatable` interfaces. */
public interface IEncodeArguments : IBaseArguments, IValidatable
{
}