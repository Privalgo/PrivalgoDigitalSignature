namespace PrivalgoDigitalSignature.Types;

[Flags]
/* The code is defining an enumeration called `CommandType`. Each value in the enumeration represents a
different command type. */
public enum CommandType
{
    Hash = 1,
    Sign = 2,
    Encode = 4,

    HashSignEncode = Hash | Sign | Encode
}
