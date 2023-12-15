# PrivalgoDigitalSignature

.NET 8 sample project to create a DigitalSignature by hashing, signing and encoding an imput message, adding it to the message before calling a sandbox endpoint.

All information provided is provided "as is" and without any implied warranty, representation, condition or otherwise, regarding its accuracy or completeness.

## Description

This is sample code for reference only, and for testing the DigitalSignature process against the sandbox environment.

## Availability

C# project targetting .NET 8.

``` cmd
dotnet run -- [command] [arguments]
```

## Operation

3 stage process for generating a valid DigitalSignature.

- `Hash` - The input payload is UTF8 encoded and a SHA256 hash generated against that encoded data.
- `Sign` - The SHA256 encoded data is then signed using a Private Key either from KeyVault or a local file.
- `Encode` - The signature is then Base64 encoded for unambiguous transportion over HTTP(s).

Each stage can be invoked independently or all together.

## Quickstart

```cmd
REM Hash Sign and Encode using KeyVault
dotnet run -- HashSignEncode -d "{ payload data }" -p AzureKeyVault -v "Url=https://keyvault.azure.net;KeyName=KeyName;ClientId=cliedId;ClientSecret=clientSecret"

REM Hash Sign and Encode using a local Private Key
dotnet run -- HashSignEncode -f [filename] -p FileName -k "C:\PrivateKey.pem"
```

- -d / -f are interchangeable - input data from either the command line or a file
- -p [provider] - signing provider (`FileName` / `AzureKeyVault`)
- -v [connectionstring] - connection string to Azure KeyVault (`Url=;KeyName=;ClientId=;ClientSecret=;`)

## i.e.

```cmd
> dotnet run -- HashSignEncode -d "{ \"Message\": \"test\" }" -k "..\Data\DigitalSignature.Testing.key"

## Command Line Reference

```cmd
dotnet run -- --help
```
