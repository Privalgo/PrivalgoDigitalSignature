using PrivalgoDigitalSignature.Parameters.Interfaces;
using PrivalgoDigitalSignature.SigningProviders.Interfaces;
using PrivalgoDigitalSignature.Types;

namespace PrivalgoDigitalSignature.SigningProviders;

/* The `SigningProviderFactory` class provides a method to get an instance of an object that implements
the `ISigningProvider` interface based on the provided arguments. */
public class SigningProviderFactory
{
    /// <summary>
    /// The function `GetSigningProvider` returns an instance of `ISigningProvider` based on the
    /// provided `ISignArguments` object.
    /// </summary>
    /// <param name="ISignArguments">ISignArguments is an interface that represents the arguments
    /// required for signing a document. It likely contains properties such as SigningProvider (an enum
    /// indicating the type of signing provider), PrivateKeyFileName (a string representing the file
    /// name of the private key), and AzureKeyVaultConnectionString (a string representing the
    /// connection</param>
    /// <returns>
    /// The method is returning an instance of an object that implements the ISigningProvider interface.
    /// The specific implementation depends on the value of the SigningProvider property of the
    /// arguments parameter. If the value is SigningProviderType.FileName, a
    /// PrivateKeyFileSigningProvider object is returned. If the value is
    /// SigningProviderType.AzureKeyVault, an AzureKeyVaultSigningProvider object is returned. If the
    /// value is any
    /// </returns>
    public static ISigningProvider GetSigningProvider(ISignArguments arguments)
    {
        switch (arguments.SigningProvider)
        {
            case SigningProviderType.FileName:
                return new PrivateKeyFileSigningProvider(arguments.PrivateKeyFileName);

            case SigningProviderType.AzureKeyVault:
                return new AzureKeyVaultSigningProvider(arguments.AzureKeyVaultConnectionString);

            default:
                throw new ArgumentOutOfRangeException(nameof(arguments), $"Unable to locate {nameof(ISigningProvider)} for {arguments.SigningProvider}");
        }
    }
}
