using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Fakebook.Feed.Api.Security;

public static class RsaSecurityKeyFactory
{
    public static RsaSecurityKey CreatePublicKey(string publicKeyPem)
    {
        if (string.IsNullOrWhiteSpace(publicKeyPem))
        {
            throw new InvalidOperationException("JWT public key configuration is missing.");
        }

        var rsa = RSA.Create();
        rsa.ImportFromPem(publicKeyPem.Replace("\\n", "\n", StringComparison.Ordinal));

        return new RsaSecurityKey(rsa);
    }
}
