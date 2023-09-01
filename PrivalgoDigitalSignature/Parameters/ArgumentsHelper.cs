namespace PrivalgoDigitalSignature.Parameters;

/* The ArgumentsHelper class provides a method to retrieve either provided data or the content of a
file specified by its filename. */
public class ArgumentsHelper
{
    /// <summary>
    /// The function `GetDataOrFileContent` returns either the provided data or the content of a file
    /// specified by its filename.
    /// </summary>
    /// <param name="data">The "data" parameter is a string that represents the data that you want to
    /// retrieve. It can be any text or content that you want to access.</param>
    /// <param name="dataFileName">The dataFileName parameter is a string that represents the name or
    /// path of a file that contains the data.</param>
    /// <returns>
    /// The method is returning a string.
    /// </returns>
    public static string GetDataOrFileContent(string data, string dataFileName)
    {
        if (string.IsNullOrWhiteSpace(data) && string.IsNullOrWhiteSpace(dataFileName))
            throw new Exception("Must specify Data or DataFileName");

        if (!string.IsNullOrWhiteSpace(data))
            return data;

        var fileInfo = new FileInfo(dataFileName);
        if (!fileInfo.Exists)
            throw new Exception($"File does not exist - {dataFileName}");

        return File.ReadAllText(fileInfo.FullName);
    }
}
