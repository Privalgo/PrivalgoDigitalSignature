using System.Security.Cryptography;
using PrivalgoDigitalSignature.Helpers;
using PrivalgoDigitalSignature.SigningProviders.Interfaces;

namespace PrivalgoDigitalSignature.SigningProviders;

/* The PrivateKeyFileSigningProvider class is a C# implementation of the ISigningProvider interface
that uses a private key file to sign a hash using RSA encryption. */
public class PrivateKeyFileSigningProvider : ISigningProvider
{
    private readonly RSA _rsa;
    private string fileName;

    public string FileName { get => fileName; set => fileName = value; }

    /* The `public PrivateKeyFileSigningProvider(string fileName)` is a constructor for the
    `PrivateKeyFileSigningProvider` class. It takes a `fileName` parameter, which represents the
    path to a private key file. */
    public PrivateKeyFileSigningProvider(string fileName)
    {
        FileName = fileName;

        _rsa = CryptographyHelper.ImportPem(File.ReadAllText(FileName));
    }

    /// <summary>
    /// The function SignHash takes a byte array as input, signs it using RSA with SHA256 hashing and
    /// PKCS1 padding, and returns the signed byte array.
    /// </summary>
    /// <param name="bytes">The "bytes" parameter is a byte array that represents the hash value of the
    /// data that you want to sign.</param>
    /// <returns>
    /// The method is returning a byte array, which represents the signed hash of the input bytes.
    /// </returns>
    public byte[] SignHash(byte[] bytes)
    {
        var signedBytes = _rsa.SignHash(bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        return signedBytes;
    }
}
