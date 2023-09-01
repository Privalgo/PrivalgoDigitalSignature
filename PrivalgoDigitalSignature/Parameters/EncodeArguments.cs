using CommandLine;
using PrivalgoDigitalSignature.Parameters.Interfaces;
using PrivalgoDigitalSignature.Types;

namespace PrivalgoDigitalSignature.Parameters;

[Verb(name: nameof(CommandType.Encode), HelpText = V)]
/* The `EncodeArguments` class is used to encode signed data using Base64 and provides options for
specifying the data directly or through a file. */
public class EncodeArguments : IEncodeArguments
{
    private const string V = "Encode signed data using Base64";

    [Option('d', "data", Required = false, HelpText = "The data to be processed")]
    public required string Data { get; set; }

    [Option('f', "datafilename", Required = false, HelpText = "The filename containing data to be processed")]
    public string? DataFileName { get; set; }

    public string InputData => ArgumentsHelper.GetDataOrFileContent(Data, dataFileName: DataFileName);
    
    public void Validate()
    {
    }
}