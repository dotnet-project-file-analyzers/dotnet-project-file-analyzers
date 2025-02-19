namespace DotNetProjectFile.Licensing;

public sealed record PermissiveLicense : SingleLicense
{
    public PermissiveLicense(string identifier, string? baseLicense = null, ImmutableArray<string>? deprecated = null)
        : base(identifier, baseLicense, deprecated ?? [])
    {

    }

    public override bool CompatibleWith(LicenseExpression other)
        => true;
}
