using System.Security.Cryptography;

namespace DotNetProjectFile.Licensing;

public sealed record CustomLicense(string Hash) : LicenseExpression
{
    public override string Expression => "Custom";

    public override bool SpdxCompliant => false;

    public override bool CompatibleWith(LicenseExpression other) => false;

    public static CustomLicense Create(string content)
    {
        var hash = SHA.ComputeHash(System.Text.Encoding.UTF8.GetBytes(content));
        return new(Convert.ToBase64String(hash).TrimEnd('='));
    }

    private static readonly SHA1 SHA = SHA1Managed.Create();
}
