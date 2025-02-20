namespace DotNetProjectFile.Licensing;

public sealed record PermissiveLicense : SingleLicense
{
    public PermissiveLicense(
        string identifier,
        string? baseLicense = null,
        ImmutableArray<string>? deprecated = null,
        bool spdxCompliant = true)
        : base(
            identifier: identifier,
            baseLicense: baseLicense,
            deprecated: deprecated ?? [],
            spdxCompliant: spdxCompliant)
    {

    }

    public override bool CompatibleWith(LicenseExpression other)
        => true;
}
