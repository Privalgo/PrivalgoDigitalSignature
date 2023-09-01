using PrivalgoDigitalSignature.Types;

namespace PrivalgoDigitalSignature.Parameters.Interfaces;

/* The code is defining a public interface named `ISignArguments`. This interface inherits from two
other interfaces, `IBaseArguments` and `IValidatable`. */
public interface ISignArguments : IBaseArguments, IValidatable
{
    SigningProviderType SigningProvider { get; set; }
    string PrivateKeyFileName { get; set; }
    string AzureKeyVaultConnectionString { get; set; }
}