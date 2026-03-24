using System.Security.Cryptography;
using System.Text;

namespace DotNetProjectFile.Licensing;

public sealed record CustomLicense : LicenseExpression
{
    [ThreadStatic]
    private static IncrementalHash? sha256;

    private CustomLicense(string hash) => Hash = hash;

    /// <summary>The SHA256 of the license text.</summary>
    public string Hash { get; }

    /// <inheritdoc />
    public override string Expression => "Custom";

    /// <inheritdoc />
    public override bool SpdxCompliant => false;

    /// <inheritdoc />
    public override bool CompatibleWith(LicenseExpression other) => false;

    /// <summary>Creates a <see cref="CustomLicense"/> based on the license text.</summary>
    public static CustomLicense Create(string content)
    {
        sha256 ??= IncrementalHash.CreateHash(HashAlgorithmName.SHA256);

        sha256.AppendData(Encoding.UTF8.GetBytes(content));
        var hash = sha256.GetHashAndReset();
        var truncated = hash.AsSpan(0, 16).ToArray();

        return new(Convert.ToBase64String(truncated).Replace('/', '_').TrimEnd('='));
    }
}
