namespace DotNetProjectFile.Licensing;

public sealed record AndLicenseExpression(LicenseExpression Left, LicenseExpression Right) : LicenseExpression()
{
    public override string Expression => $"({Left} AND {Right})";

    public override bool CompatibleWith(LicenseExpression other)
        => Left.CompatibleWith(other) && Right.CompatibleWith(other);
}
