using CommandLine;
using PrivalgoDigitalSignature.Exceptions;
using PrivalgoDigitalSignature.Parameters.Interfaces;
using PrivalgoDigitalSignature.Types;

namespace PrivalgoDigitalSignature.Parameters;

[Verb(nameof(CommandType.HashSignEncode), HelpText = "Hash, Sign and Encode input data")]
/* The `HashSignEncodeArguments` class is a C# class that represents the arguments needed for hash sign
encoding, including data, data file name, signing provider type, private key file name, and Azure
KeyVault connection string. */
public class HashSignEncodeArguments : IHashSignEncodeArguments
{
    [Option('d', "data", Required = false, HelpText = "The data to be processed")]
    public required string Data { get; set; }

    [Option('f', "datafilename", Required = false, HelpText = "The filename containing data to be processed")]
    public string? DataFileName { get; set; }

    public string InputData => ArgumentsHelper.GetDataOrFileContent(Data, DataFileName);
    [Option('p', "provider", Required = false, Default = SigningProviderType.FileName, HelpText = "The Provider to use for Signing requests")]
    public SigningProviderType SigningProvider { get; set; }

    [Option('k', "keyfilename", Required = false, Default = null, HelpText = "FileName for Private Key")]
    public string? PrivateKeyFileName { get; set; }

    [Option('v', "keyvault", Required = false, Default = null, HelpText = "Azure KeyVault connection string")]
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
