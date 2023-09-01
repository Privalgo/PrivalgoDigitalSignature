namespace PrivalgoDigitalSignature.SigningProviders.Interfaces;

/* The code is defining an interface named `ISigningProvider`. An interface is a contract that defines
a set of methods that a class must implement. */
public interface ISigningProvider
{
    byte[] SignHash(byte[] bytes);
}
