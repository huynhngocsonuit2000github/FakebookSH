using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Fakebook.Auth.Infrastructure.Security;

public static class RsaSecurityKeyFactory
{
    public static RsaSecurityKey CreatePrivateKey(string privateKeyPem)
    {
        return CreateKey(privateKeyPem, "JWT private key");
    }

    public static RsaSecurityKey CreatePublicKey(string publicKeyPem)
    {
        return CreateKey(publicKeyPem, "JWT public key");
    }

    private static RsaSecurityKey CreateKey(string pem, string optionName)
    {
        if (string.IsNullOrWhiteSpace(pem))
        {
            throw new InvalidOperationException($"{optionName} configuration is missing.");
        }

        var rsa = RSA.Create();
        rsa.ImportFromPem(NormalizePem(pem));

        return new RsaSecurityKey(rsa);
    }

    private static string NormalizePem(string pem)
    {
        return pem.Replace("\\n", "\n", StringComparison.Ordinal);
    }
}
