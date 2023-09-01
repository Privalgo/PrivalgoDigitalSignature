namespace PrivalgoDigitalSignature.Parameters.Interfaces;

/* The code is defining a C# interface called `IHashSignEncodeArguments`. This interface inherits from
three other interfaces: `IHashArguments`, `ISignArguments`, and `IEncodeArguments`. By inheriting
from these interfaces, `IHashSignEncodeArguments` will inherit all the properties and methods
defined in those interfaces. This allows `IHashSignEncodeArguments` to have access to the
functionality defined in `IHashArguments`, `ISignArguments`, and `IEncodeArguments`, making it a
combination of all three interfaces. */
public interface IHashSignEncodeArguments : IHashArguments, ISignArguments, IEncodeArguments
{
}