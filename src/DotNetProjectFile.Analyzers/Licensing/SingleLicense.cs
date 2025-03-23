namespace DotNetProjectFile.Licensing;

public abstract record SingleLicense : LicenseExpression
{
    protected SingleLicense(string identifier, string? baseLicense, ImmutableArray<string> deprecated, bool spdxCompliant)
    {
        Expression = identifier;
        BaseLicense = baseLicense;
        Deprecated = deprecated;
        SpdxInfo = Spdx.TryGetLicense(identifier);
        SpdxCompliant = spdxCompliant && SpdxInfo is { };
    }

    public override string Expression { get; }

    public ImmutableArray<string> Deprecated { get; }

    public string? BaseLicense { get; }

    public SpdxLicenseInfo? SpdxInfo { get; }

    public override bool SpdxCompliant { get; }
}
