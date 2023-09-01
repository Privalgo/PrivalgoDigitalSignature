namespace PrivalgoDigitalSignature.Results;

/* The `RunResult` class represents the result of a data processing operation, including raw data,
encoded data, hashed data, and signed data. */
public class RunResult
{
    public string? RawData { get; set; }

    public byte[]? UTF8EncodedData { get; set; }

    public byte[]? HashedData { get; set; }

    public byte[]? SignedData { get; set; }

    public string? EncodedData { get; set; }
}
