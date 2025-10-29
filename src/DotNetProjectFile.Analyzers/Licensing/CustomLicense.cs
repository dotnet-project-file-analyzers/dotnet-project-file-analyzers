using System.Security.Cryptography;
using System.Text;

namespace DotNetProjectFile.Licensing;

public sealed record CustomLicense(string Hash) : LicenseExpression
{
    [ThreadStatic]
    private static IncrementalHash? sha256;

    public override string Expression => "Custom";

    public override bool SpdxCompliant => false;

    public override bool CompatibleWith(LicenseExpression other) => false;

    public static CustomLicense Create(string content)
    {
        sha256 ??= IncrementalHash.CreateHash(HashAlgorithmName.SHA256);

        sha256.AppendData(Encoding.UTF8.GetBytes(content));
        var hash = sha256.GetHashAndReset();
        var truncated = hash.AsSpan(0, 16).ToArray();

        return new(Convert.ToBase64String(truncated).Replace('/', '_').TrimEnd('='));
    }
}
