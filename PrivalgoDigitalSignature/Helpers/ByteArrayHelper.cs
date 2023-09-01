namespace PrivalgoDigitalSignature.Helpers;

/* The ByteArrayHelper class provides methods to convert a string of comma-separated byte values into a
byte array and to describe a byte array as a string of comma-separated byte values. */
public class ByteArrayHelper
{
    /// <summary>
    /// The function takes a string of comma-separated values and converts them into an array of bytes.
    /// </summary>
    /// <param name="text">The "text" parameter is a string that represents a comma-separated list of
    /// byte values.</param>
    /// <returns>
    /// The method is returning a byte array.
    /// </returns>
    public static byte[] TranslateByteArray(string text)
    {
        var values = (text ?? string.Empty).Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .Select(byte.Parse)
            .ToArray();

        return values;
    }

    /// <summary>
    /// The function "DescribeByteArray" takes in a collection of bytes and returns a string
    /// representation of the bytes separated by commas.
    /// </summary>
    /// <param name="bytes">The "bytes" parameter is an IEnumerable of bytes. It represents a collection
    /// of bytes that you want to describe.</param>
    /// <returns>
    /// The method returns a string that describes the given byte array.
    /// </returns>
    public static string DescribeByteArray(IEnumerable<byte> bytes)
    {
        var text = string.Join(",", bytes?.Select(b => b.ToString()) ?? Enumerable.Empty<string>());

        return text;
    }
}
