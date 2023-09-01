namespace PrivalgoDigitalSignature.Types;

/* The code snippet is defining an enumeration called `SigningProviderType`. An enumeration is a set of
named values that represent a set of possible options or choices. In this case, the
`SigningProviderType` enumeration has two possible values: `FileName` and `AzureKeyVault`. This
enumeration can be used to specify the type of signing provider in a digital signature system. */
public enum SigningProviderType
{
    FileName,

    AzureKeyVault
}
