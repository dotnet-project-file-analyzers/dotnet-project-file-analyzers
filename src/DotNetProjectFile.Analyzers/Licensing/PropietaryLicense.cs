namespace DotNetProjectFile.Licensing;

public sealed record PropietaryLicense: SingleLicense
{
    public PropietaryLicense(
        string identifier,
        Func<LicenseExpression, bool>? compatibleWith = null,
        string? baseLicense = null,
        ImmutableArray<string>? deprecated = null,
        bool spdxCompliant = false)
        : base(
        identifier: identifier,
        baseLicense: baseLicense,
        deprecated: deprecated ?? [],
        spdxCompliant: spdxCompliant)
    {
        this.compatibleWith = compatibleWith ?? (other => other == this);
    }

    private readonly Func<LicenseExpression, bool> compatibleWith;

    public override bool CompatibleWith(LicenseExpression other)
        => compatibleWith(other);
}
