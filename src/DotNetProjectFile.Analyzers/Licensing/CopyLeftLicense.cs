namespace DotNetProjectFile.Licensing;

public sealed record CopyLeftLicense : SingleLicense
{
    public CopyLeftLicense(
        string identifier,
        string? baseLicense = null,
        ImmutableArray<string>? deprecated = null,
        ImmutableArray<string>? compatibilities = null)
        : base(identifier, baseLicense, deprecated ?? [])
    {
        Compatibilities = compatibilities ?? [];
    }

    public ImmutableArray<string> Compatibilities { get; }

    public override bool CompatibleWith(LicenseExpression other)
    {
        if (other.Expression == Expression || Compatibilities.Contains(other.Expression))
        {
            return true;
        }

        return other switch
        {
            // NB: the inversion of the `and` and `or` are intentional.
            AndLicenseExpression e => CompatibleWith(e.Left) || CompatibleWith(e.Right),
            OrLicenseExpression e => CompatibleWith(e.Left) && CompatibleWith(e.Right),
            _ => false, // All other cases.
        };
    }
}
