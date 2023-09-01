using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using CommandLine;
using PrivalgoDigitalSignature.Extensions;
using PrivalgoDigitalSignature.Helpers;
using PrivalgoDigitalSignature.Parameters;
using PrivalgoDigitalSignature.Parameters.Interfaces;
using PrivalgoDigitalSignature.Results;
using PrivalgoDigitalSignature.SigningProviders;
using PrivalgoDigitalSignature.SigningProviders.Interfaces;

namespace Privalgo.API.DigitalSignature;

/* The `Program` class is a C# console application that parses command line arguments and executes
different operations based on the provided arguments. */
internal class Program
{
    /* The CBResponse class is an internal class in C# that has a public integer property called Nonce. */
    internal class CBResponse
    {
        public int Nonce;
    }

    /// <summary>
    /// The above function is the entry point of a C# console application that parses command line
    /// arguments and executes different operations based on the provided arguments.
    /// </summary>
    /// <param name="args">The `args` parameter is an array of strings that represents the command-line
    /// arguments passed to the program when it is executed.</param>
    /// <returns>
    /// The method is returning an integer value.
    /// </returns>
    private static int Main(string[] args)
    {
        try
        {
            RunResult runResult = new();

            Parser parser = new(
                config =>
                {
                    config.CaseInsensitiveEnumValues = true;
                    config.CaseSensitive = false;
                    config.IgnoreUnknownArguments = true;
                    config.AutoHelp = true;
                    config.AutoVersion = true;
                    config.MaximumDisplayWidth = Console.IsOutputRedirected ? int.MaxValue : Console.WindowWidth;
                    config.HelpWriter = Console.Out;
                }
            );

            int result = parser.ParseArguments<HashArguments, SignArguments, EncodeArguments, HashSignEncodeArguments>(args)
                .MapResult(
                    (HashArguments arguments) => HashMessage(arguments, runResult),
                    (SignArguments arguments) => SignMessage(arguments, runResult),
                    (EncodeArguments arguments) => EncodeMessage(arguments, runResult),
                    (HashSignEncodeArguments arguments) => HashSignEncodeMessage(arguments, runResult),
                    errs => 1
                );


            SendMessage(runResult.RawData, runResult.EncodedData);

            return result;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine();
            Console.Error.WriteLine($"Error: {ex.Message}");

            return 2;
        }
        finally
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                while (Console.KeyAvailable)
                    Console.ReadKey(true);

                Console.Out.Write("Press any key to exit...");
                Console.ReadKey(true);
                Console.Out.WriteLine();
            }
        }
    }

    /// <summary>
    /// The `SendMessage` function sends an HTTP POST request with a JSON payload and a digital
    /// signature to a specified API endpoint.
    /// </summary>
    /// <param name="inputData">The `inputData` parameter is a string that represents the payload data
    /// that you want to send in the HTTP request. It should be in JSON format.</param>
    /// <param name="digitalSignature">The `digitalSignature` parameter is a string that represents the
    /// digital signature of the `inputData`. It is used for verifying the integrity and authenticity of
    /// the data being sent in the HTTP request. The digital signature is typically generated using a
    /// cryptographic algorithm and a private key.</param>
    private static async void SendMessage(string inputData, string digitalSignature)
    {
        AuthProfile _authProfile = new()
        {
            ApiUrl = @"[SandboxApiURL]",
            ApiToken = @"[ApiToken]"
        };

        HttpRequestMessage requestMessage = new(HttpMethod.Post, "v1/test");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authProfile.ApiToken);

        // It is important to sign exactly same payload that is going to be send in the http request
        // Conversion from object to JSON in different places may result in different strings and request will fail digital signature validation
        requestMessage.Content = new StringContent(inputData, Encoding.UTF8, "application/json");
        requestMessage.Headers.Add("DigitalSignature", digitalSignature);

        // X-Request-Id - unique string that identifies the request. Do not reuse the same in 24 hour period.
        // If your request result in server error, use the same X-Request-Id for retries
        requestMessage.Headers.Add("X-Request-Id", Guid.NewGuid().ToString("N"));

        using HttpClient client = new() { BaseAddress = new Uri(_authProfile.ApiUrl) };
        HttpResponseMessage response = await client.SendAsync(requestMessage);

        // You should save X-Correlation-Id for future reference
        // If you have any questions about your request our support will ask you to provide X-Request-Id and X-Correlation-Id
        response.Headers.TryGetValues("X-Correlation-Id", out IEnumerable<string>? headers);
        string? correlationId = headers?.First();
        string body = await response.Content.ReadAsStringAsync();

        Console.WriteLine(body);
    }

    /// <summary>
    /// The function `HashMessage` takes in an `IHashArguments` object and a `RunResult` object,
    /// validates the arguments, hashes the input data using SHA256, and returns 0.
    /// </summary>
    /// <param name="IHashArguments">IHashArguments is an interface that represents the arguments for
    /// hashing a message. It likely contains properties such as:</param>
    /// <param name="RunResult">RunResult is a class that contains the result of running a hash
    /// operation. It has the following properties:</param>
    /// <returns>
    /// The method is returning an integer value of 0.
    /// </returns>
    private static int HashMessage(IHashArguments arguments, RunResult runResult)
    {
        Console.Out.WriteLine();
        Console.Out.WriteLine($"{nameof(HashMessage)}");

        arguments.Validate();

        if (!string.IsNullOrEmpty(arguments.InputData) &&
            string.IsNullOrEmpty(runResult.RawData))
        {
            runResult.RawData = arguments.InputData;
        }

        Console.Out.WriteLine($"Using: {arguments.InputData}");

        runResult.UTF8EncodedData = Encoding.UTF8.GetBytes(arguments.InputData);
        Console.Out.WriteLine($"{nameof(RunResult.UTF8EncodedData)}: {ByteArrayHelper.DescribeByteArray(runResult.UTF8EncodedData)}");

        using SHA256Managed hashProvider = new();
        runResult.HashedData = hashProvider.ComputeHash(runResult.UTF8EncodedData);
        Console.Out.WriteLine($"{nameof(RunResult.HashedData)} (SHA256): {ByteArrayHelper.DescribeByteArray(runResult.HashedData)}");

        return 0;
    }

    /// <summary>
    /// The function `SignMessage` signs a message using a signing provider and returns the signed data.
    /// </summary>
    /// <param name="ISignArguments">ISignArguments is an interface that represents the arguments for
    /// signing a message. It likely contains properties such as InputData, which is the data to be
    /// signed, and any other necessary parameters for the signing process.</param>
    /// <param name="RunResult">RunResult is a class that contains the result of a running process. It
    /// has the following properties:</param>
    /// <returns>
    /// The method is returning an integer value of 0.
    /// </returns>
    private static int SignMessage(ISignArguments arguments, RunResult runResult)
    {
        Console.Out.WriteLine();
        Console.Out.WriteLine($"{nameof(SignMessage)}");

        arguments.Validate();

        if (!string.IsNullOrEmpty(arguments.InputData) &&
            string.IsNullOrEmpty(runResult.RawData))
        {
            runResult.RawData = arguments.InputData;
        }

        byte[] inputData = runResult.HashedData.HasAny()
            ? runResult.HashedData
            : ByteArrayHelper.TranslateByteArray(arguments.InputData);

        Console.Out.WriteLine($"Using: {ByteArrayHelper.DescribeByteArray(inputData)}");

        ISigningProvider signingProvider = SigningProviderFactory.GetSigningProvider(arguments);

        runResult.SignedData = signingProvider.SignHash(inputData);

        Console.Out.WriteLine($"{nameof(RunResult.SignedData)}: {ByteArrayHelper.DescribeByteArray(runResult.SignedData)}");

        return 0;
    }

    /// <summary>
    /// The function `EncodeMessage` takes in arguments and a run result, validates the arguments,
    /// converts the input data to a byte array, encodes the byte array as a base64 string, and returns
    /// 0.
    /// </summary>
    /// <param name="IEncodeArguments">IEncodeArguments is an interface that represents the arguments
    /// for encoding a message. It likely contains properties such as InputData, which is the input
    /// message to be encoded.</param>
    /// <param name="RunResult">RunResult is a class that contains various properties related to the
    /// result of a run. In this context, it is used to store the encoded data.</param>
    /// <returns>
    /// The method is returning an integer value of 0.
    /// </returns>
    private static int EncodeMessage(IEncodeArguments arguments, RunResult runResult)
    {
        Console.Out.WriteLine();
        Console.Out.WriteLine($"{nameof(EncodeMessage)}");

        arguments.Validate();

        if (!string.IsNullOrEmpty(arguments.InputData) &&
            string.IsNullOrEmpty(runResult.RawData))
        {
            runResult.RawData = arguments.InputData;

        }
        byte[] inputData = runResult.SignedData.HasAny()
            ? runResult.SignedData
            : ByteArrayHelper.TranslateByteArray(arguments.InputData);

        Console.Out.WriteLine($"Using: {ByteArrayHelper.DescribeByteArray(inputData)}");

        runResult.EncodedData = Convert.ToBase64String(inputData);
        Console.Out.WriteLine($"{nameof(RunResult.EncodedData)}: {runResult.EncodedData}");

        return 0;
    }

    /// <summary>
    /// The function HashSignEncodeMessage takes in arguments and a run result, validates the arguments,
    /// sets the raw data in the run result if it is not already set, and then calls three other
    /// functions to hash, sign, and encode the message.
    /// </summary>
    /// <param name="IHashSignEncodeArguments">The IHashSignEncodeArguments is an interface that defines
    /// the arguments required for the HashSignEncodeMessage method. It likely contains properties or
    /// methods that provide access to the input data and other necessary information for the method to
    /// perform its tasks.</param>
    /// <param name="RunResult">The RunResult parameter is an object that stores the result of the
    /// execution of the method. It contains properties such as RawData, which stores the raw data of
    /// the message, and other properties that may be used to store intermediate or final results of the
    /// hashing, signing, and encoding operations.</param>
    /// <returns>
    /// The method is returning an integer value of 0.
    /// </returns>
    private static int HashSignEncodeMessage(IHashSignEncodeArguments arguments, RunResult runResult)
    {
        arguments.Validate();

        if (!string.IsNullOrEmpty(arguments.InputData) &&
            string.IsNullOrEmpty(runResult.RawData))
        {
            runResult.RawData = arguments.InputData;
        }

        HashMessage(arguments, runResult);
        SignMessage(arguments, runResult);
        EncodeMessage(arguments, runResult);

        return 0;
    }
}
