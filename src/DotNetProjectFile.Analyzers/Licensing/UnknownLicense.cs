namespace DotNetProjectFile.Licensing;

public sealed record UnknownLicense : LicenseExpression
{
    public static readonly UnknownLicense Instance = new();

    private UnknownLicense()
    {
    }

    public override string Expression => string.Empty;

    public override bool CompatibleWith(LicenseExpression other)
        => true;
}
