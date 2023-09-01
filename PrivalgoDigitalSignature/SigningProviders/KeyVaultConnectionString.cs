using PrivalgoDigitalSignature.Parameters.Interfaces;

namespace PrivalgoDigitalSignature.SigningProviders;

/* The `KeyVaultConnectionString` class represents a connection string for accessing a key vault, with
properties for the URL, key name, key version, algorithm, client ID, and client secret, and a method
for validating the connection string. */
public class KeyVaultConnectionString : IValidatable
{
    public const string DefaultAlgorithm = "RS256";
    public const string DefaultKeyVersion = "";

    public string Url { get; set; }
    public string KeyName { get; set; }
    public string KeyVersion { get; set; }
    public string Algorithm { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }

    public bool HasKeyName => !string.IsNullOrWhiteSpace(KeyName);
    public bool HasKeyVersion => !string.IsNullOrWhiteSpace(KeyVersion);

    /* The `KeyVaultConnectionString` class has a constructor that takes a `connectionString`
    parameter. This constructor is responsible for parsing the connection string and initializing
    the properties of the `KeyVaultConnectionString` object. */
    public KeyVaultConnectionString(string connectionString)
    {
        var dict = (connectionString ?? string.Empty).Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Split("=".ToCharArray()))
            .ToDictionary(x => x.First().ToLower(System.Globalization.CultureInfo.CurrentCulture), x => string.Join("=", x.Skip(1)));

        Url = GetValue(dict, nameof(Url).ToLower(System.Globalization.CultureInfo.CurrentCulture));
        KeyName = GetValue(dict, nameof(KeyName).ToLower(System.Globalization.CultureInfo.CurrentCulture));
        KeyVersion = GetValue(dict, nameof(KeyVersion).ToLower(System.Globalization.CultureInfo.CurrentCulture)) ?? DefaultKeyVersion;
        Algorithm = GetValue(dict, nameof(Algorithm).ToLower(System.Globalization.CultureInfo.CurrentCulture)) ?? DefaultAlgorithm;
        ClientId = GetValue(dict, nameof(ClientId).ToLower(System.Globalization.CultureInfo.CurrentCulture));
        ClientSecret = GetValue(dict, nameof(ClientSecret).ToLower(System.Globalization.CultureInfo.CurrentCulture));
    }

    /// <summary>
    /// The function `GetValue` takes a dictionary and a key as input, and returns the corresponding
    /// value from the dictionary if the key exists, otherwise it returns null.
    /// </summary>
    /// <param name="dict">An IDictionary<string, string> object representing a dictionary with string
    /// keys and string values.</param>
    /// <param name="key">The key parameter is a string that represents the key of the value we want to
    /// retrieve from the dictionary.</param>
    /// <returns>
    /// The method is returning a string value from the dictionary if the key exists, otherwise it
    /// returns null.
    /// </returns>
    private static string? GetValue(IDictionary<string, string> dict, string key)
    {
        return dict.ContainsKey(key)
            ? dict[key]
            : null;
    }

    /// <summary>
    /// The Validate function checks if certain properties are null, empty, or whitespace and throws an
    /// ArgumentException if any of them are.
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Url))
            throw new ArgumentException($"{nameof(KeyVaultConnectionString)}: Invalid or missing {nameof(Url)}", nameof(Url));
        if (string.IsNullOrWhiteSpace(KeyName))
            throw new ArgumentException($"{nameof(KeyVaultConnectionString)}: Invalid or missing {nameof(KeyName)}", nameof(KeyName));
        if (string.IsNullOrWhiteSpace(ClientId))
            throw new ArgumentException($"{nameof(KeyVaultConnectionString)}: Invalid or missing {nameof(ClientId)}", nameof(ClientId));
        if (string.IsNullOrWhiteSpace(ClientSecret))
            throw new ArgumentException($"{nameof(KeyVaultConnectionString)}: Invalid or missing {nameof(ClientSecret)}", nameof(ClientSecret));
    }
}
