namespace DotNetProjectFile.Licensing;

public sealed record UnknownLicense : LicenseExpression
{
    public static readonly UnknownLicense Instance = new();

    private UnknownLicense() { }

    public override bool IsKnown => false;

    public override string Expression => "NOASSERTION";

    public override bool SpdxCompliant => false;

    public override bool CompatibleWith(LicenseExpression other) => true;
}
