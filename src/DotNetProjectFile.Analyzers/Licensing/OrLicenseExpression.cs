namespace DotNetProjectFile.Licensing;

public sealed record OrLicenseExpression(LicenseExpression Left, LicenseExpression Right) : LicenseExpression()
{
    public override string Expression => $"({Left} OR {Right})";

    public override bool SpdxCompliant => Left.SpdxCompliant && Right.SpdxCompliant;

    public override bool CompatibleWith(LicenseExpression other)
        => Left.CompatibleWith(other) || Right.CompatibleWith(other);
}
