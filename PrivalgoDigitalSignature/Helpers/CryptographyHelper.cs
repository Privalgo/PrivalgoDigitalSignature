using System.Security.Cryptography;
using System.Text;

namespace PrivalgoDigitalSignature.Helpers;

/* The `CryptographyHelper` class provides methods for generating and verifying digital signatures
using RSA encryption. */
public class CryptographyHelper
{
    /// <summary>
    /// The function generates a digital signature for a given body of text using a private key.
    /// </summary>
    /// <param name="body">The "body" parameter is a string that represents the content or data that you
    /// want to generate a digital signature for. It could be any text or data that you want to ensure
    /// integrity and authenticity for.</param>
    /// <param name="privateKeyText">The `privateKeyText` parameter is a string that represents the
    /// private key used for generating the digital signature. It is typically in a specific format,
    /// such as PEM (Privacy-Enhanced Mail) format, which is a common format for storing cryptographic
    /// keys. The `ImportPem` method is</param>
    /// <returns>
    /// The method is returning a byte array, which represents the digital signature of the provided
    /// body.
    /// </returns>
    public static byte[] GenerateDigitalSignature(string body, string privateKeyText)
    {
        var hash = HashBody(body);

        using var rsa = ImportPem(privateKeyText);
        var signedHash = rsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        return signedHash;
    }

    /// <summary>
    /// The function verifies a digital signature by hashing the contents, importing a public key, and
    /// then using RSA to verify the hash with the digital signature.
    /// </summary>
    /// <param name="digitalSignature">The digitalSignature parameter is a string representing the
    /// digital signature that needs to be verified.</param>
    /// <param name="contents">The `contents` parameter represents the data or message that was
    /// signed.</param>
    /// <param name="publicKeyText">The `publicKeyText` parameter is a string that represents the public
    /// key used for verifying the digital signature. It is typically in the form of a PEM
    /// (Privacy-Enhanced Mail) encoded string, which is a common format for representing cryptographic
    /// keys.</param>
    /// <returns>
    /// The method is returning a boolean value indicating whether the digital signature is verified or
    /// not.
    /// </returns>
    public static bool VerifyDigitalSignature(string digitalSignature, string contents, string publicKeyText)
    {
        var hash = HashBody(contents);

        bool verified;
        try
        {
            using var rsa = ImportPem(publicKeyText);
            verified = rsa.VerifyHash(hash, Convert.FromBase64String(digitalSignature), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        catch
        {
            verified = false;
        }

        return verified;
    }

    /// <summary>
    /// The function takes a string content, converts it to bytes using UTF8 encoding, and then computes
    /// the SHA256 hash of the content bytes.
    /// </summary>
    /// <param name="content">The `content` parameter is a string that represents the content that you
    /// want to hash.</param>
    /// <returns>
    /// The method is returning a byte array, which is the hash value of the content.
    /// </returns>
    internal static byte[] HashBody(string content)
    {
        var contentBytes = Encoding.UTF8.GetBytes(content);
        var hash = SHA256.HashData(contentBytes);

        return hash;
    }

    /// <summary>
    /// The function imports an RSA key from a PEM format string and returns the RSA object.
    /// </summary>
    /// <param name="pem">The `pem` parameter is a string that represents the PEM-encoded RSA key. PEM
    /// stands for Privacy Enhanced Mail and is a format for storing and transmitting cryptographic
    /// keys, certificates, and other data.</param>
    /// <returns>
    /// The method is returning an RSA object.
    /// </returns>
    public static RSA ImportPem(string pem)
    {
        var rsa = RSA.Create();
        try
        {
            rsa.ImportFromPem(pem);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException("Invalid PEM", nameof(pem), ex);
        }

        return rsa;
    }
}
