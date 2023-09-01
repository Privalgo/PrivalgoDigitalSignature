using CommandLine;
using PrivalgoDigitalSignature.Parameters.Interfaces;
using PrivalgoDigitalSignature.Types;

namespace PrivalgoDigitalSignature.Parameters;

[Verb(nameof(CommandType.Hash), HelpText = "Hash input text using SHA256")]
/* The HashArguments class is a C# implementation of a command-line argument parser for hashing data. */
public class HashArguments : IHashArguments
{
    [Option('d', "data", Required = false, HelpText = "The data to be processed")]
    public required string Data { get; set; }

    [Option('f', "datafilename", Required = false, HelpText = "The filename containing data to be processed")]
    public string? DataFileName { get; set; }

    public string InputData => ArgumentsHelper.GetDataOrFileContent(Data, DataFileName);
    public void Validate()
    {
    }
}