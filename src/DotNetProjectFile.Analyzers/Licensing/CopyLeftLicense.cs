namespace DotNetProjectFile.Licensing;

public sealed record CopyLeftLicense : SingleLicense
{
    public CopyLeftLicense(
        string identifier,
        string? baseLicense = null,
        ImmutableArray<string>? deprecated = null,
        ImmutableArray<string>? compatibilities = null,
        bool spdxCompliant = true)
        : base(
            identifier: identifier,
            baseLicense: baseLicense,
            deprecated: deprecated ?? [],
            spdxCompliant: spdxCompliant)
    {
        Compatibilities = compatibilities ?? [];
    }

    public ImmutableArray<string> Compatibilities { get; }

    public override bool CompatibleWith(LicenseExpression other) => other switch
    {
        _ when other.Expression == Expression
            || Compatibilities.Contains(other.Expression) => true,

        // NB: the inversion of the `and` and `or` are intentional.
        AndLicenseExpression e => CompatibleWith(e.Left) || CompatibleWith(e.Right),
        OrLicenseExpression e => CompatibleWith(e.Left) && CompatibleWith(e.Right),
        _ => false, // All other cases.
    };
}
