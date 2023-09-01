using CommandLine;
using PrivalgoDigitalSignature.Parameters.Interfaces;
using PrivalgoDigitalSignature.Exceptions;
using PrivalgoDigitalSignature.Types;

namespace PrivalgoDigitalSignature.Parameters;

[Verb(nameof(CommandType.Sign), HelpText = "Sign hashed data using a PrivateKey")]
/* The `SignArguments` class represents the arguments needed for signing requests, including data,
signing provider type, private key file name, and Azure KeyVault connection string. */
public class SignArguments : ISignArguments
{
    [Option('d', "data", Required = false, HelpText = "The data to be processed")]
    public required string Data { get; set; }

    [Option('f', "datafilename", Required = false, HelpText = "The filename containing data to be processed")]
    public string? DataFileName { get; set; }

    public string InputData => ArgumentsHelper.GetDataOrFileContent(Data, DataFileName);
    [Option('p', "provider", Required = false, Default = SigningProviderType.FileName, HelpText = "The Provider to use for Signing requests (FileName / AzureKeyVault)")]
    public SigningProviderType SigningProvider { get; set; }

    [Option('k', "keyfilename", Required = false, Default = null, HelpText = "FileName for Private Key")]
    public string? PrivateKeyFileName { get; set; }

    [Option('v', "keyvault", Required = false, Default = null, HelpText = "Azure KeyVault connection string (Url=;KeyName=;ClientId=;ClientSecret=;)")]
    public string? AzureKeyVaultConnectionString { get; set; }

    /// <summary>
    /// The function validates that the required parameters are specified based on the selected signing
    /// provider.
    /// </summary>
    public virtual void Validate()
    {
        switch (SigningProvider)
        {
            case SigningProviderType.FileName:
                if (string.IsNullOrWhiteSpace(PrivateKeyFileName))
                    throw new CommandLineParserException($"Must specify {nameof(PrivateKeyFileName)} for {nameof(SigningProvider)} {SigningProvider}");
                break;

            case SigningProviderType.AzureKeyVault:
                if (string.IsNullOrWhiteSpace(AzureKeyVaultConnectionString))
                    throw new CommandLineParserException($"Must specify {nameof(AzureKeyVaultConnectionString)} for {nameof(SigningProvider)} {SigningProvider}");
                break;
        }
    }
}
