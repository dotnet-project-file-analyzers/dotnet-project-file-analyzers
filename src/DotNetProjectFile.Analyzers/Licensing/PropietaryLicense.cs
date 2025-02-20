namespace DotNetProjectFile.Licensing;

public sealed record PropietaryLicense(
    string identifier,
    string? baseLicense = null,
    ImmutableArray<string>? deprecated = null,
    bool spdxCompliant = false)
    : SingleLicense(
        identifier: identifier,
        baseLicense: baseLicense,
        deprecated: deprecated ?? [],
        spdxCompliant: spdxCompliant)
{
    public override bool CompatibleWith(LicenseExpression other)
        => false;
}
