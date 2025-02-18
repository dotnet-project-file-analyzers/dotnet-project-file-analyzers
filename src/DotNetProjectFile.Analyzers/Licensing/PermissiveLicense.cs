namespace DotNetProjectFile.Licensing;

public sealed record PermissiveLicense : SingleLicense
{
    public PermissiveLicense(string identifier, ImmutableArray<string>? deprecated = null)
        : base(identifier, deprecated ?? [])
    {

    }

    public override bool CompatibleWith(LicenseExpression other)
        => true;
}
