using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using PrivalgoDigitalSignature.SigningProviders.Interfaces;

namespace PrivalgoDigitalSignature.SigningProviders;

/* The `AzureKeyVaultSigningProvider` class is a C# implementation of the `ISigningProvider` interface
that provides methods for signing data using Azure Key Vault. */
public class AzureKeyVaultSigningProvider : ISigningProvider
{
    private readonly KeyVaultConnectionString _connectionString;

    /* The `public AzureKeyVaultSigningProvider(string connectionString)` is a constructor for the
    `AzureKeyVaultSigningProvider` class. It takes a `connectionString` parameter, which is used to
    initialize the `_connectionString` field of type `KeyVaultConnectionString`. */
    public AzureKeyVaultSigningProvider(string connectionString)
    {
        _connectionString = new KeyVaultConnectionString(connectionString);
        _connectionString.Validate();
    }

    [Obsolete]
    /// <summary>
    /// The function returns a new instance of the KeyVaultClient class using the GetToken method as a
    /// parameter.
    /// </summary>
    /// <returns>
    /// The method is returning an instance of the KeyVaultClient class.
    /// </returns>
    private KeyVaultClient GetClient()
    {
        var keyVaultClient = new KeyVaultClient(GetToken);

        return keyVaultClient;
    }

    [Obsolete]
    /// <summary>
    /// The function `GetToken` is an asynchronous method that retrieves a JWT token from Azure Active
    /// Directory using a client credential.
    /// </summary>
    /// <param name="authority">The authority parameter is the URL of the Azure Active Directory (AAD)
    /// authority. It represents the identity provider that authenticates the user and issues the token.
    /// It typically follows the format "https://login.microsoftonline.com/{tenantId}". The tenantId is
    /// the unique identifier for the AAD tenant.</param>
    /// <param name="resource">The "resource" parameter represents the identifier of the target resource
    /// for which the token is being requested. It could be the URI of the Azure Key Vault or any other
    /// resource that requires authentication.</param>
    /// <param name="scope">The scope parameter is used to specify the permissions that the token should
    /// have. It defines the level of access that the token will have to the specified resource.</param>
    /// <returns>
    /// The method is returning a Task<string> object, which represents an asynchronous operation that
    /// will eventually produce a string result.
    /// </returns>
    protected async Task<string> GetToken(string authority, string resource, string scope)
    {
        var authContext = new AuthenticationContext(authority);

        var clientCred = new ClientCredential(
            _connectionString.ClientId,
            _connectionString.ClientSecret
        );

        var result = await authContext.AcquireTokenAsync(resource, clientCred);
        return result == null ? throw new InvalidOperationException("Failed to obtain the KeyVault JWT token") : result.AccessToken;
    }

    /// <summary>
    /// The function `SignHash` signs a byte array using an asynchronous method and returns the signed
    /// bytes.
    /// </summary>
    /// <param name="bytes">The "bytes" parameter is a byte array that represents the hash value that
    /// needs to be signed.</param>
    /// <returns>
    /// The method is returning a byte array.
    /// </returns>
    public byte[] SignHash(byte[] bytes)
    {
        var client = GetClient();

        var signedBytes = SignAsync(client, bytes)
            .GetAwaiter()
            .GetResult();

        return signedBytes.Result;
    }

    /// <summary>
    /// The function `SignAsync` signs a byte array using a key stored in Azure Key Vault and returns
    /// the result.
    /// </summary>
    /// <param name="KeyVaultClient">KeyVaultClient is a class that provides methods for interacting
    /// with Azure Key Vault. It is used to perform cryptographic operations such as signing
    /// data.</param>
    /// <param name="bytes">The "bytes" parameter is a byte array that represents the data to be signed.
    /// It is the input data that will be used to generate a digital signature.</param>
    /// <returns>
    /// The method is returning a Task of type KeyOperationResult.
    /// </returns>
    public async Task<KeyOperationResult> SignAsync(KeyVaultClient client, byte[] bytes)
    {
        KeyOperationResult result;

        if (_connectionString.HasKeyName)
        {
            result = await client.SignAsync(
                _connectionString.Url,
                _connectionString.KeyName,
                _connectionString.KeyVersion,
                _connectionString.Algorithm,
                bytes
            );
        }
        else
        {
            result = await client.SignAsync(
                _connectionString.Url,
                _connectionString.Algorithm,
                bytes
            );
        }

        return result;
    }
}