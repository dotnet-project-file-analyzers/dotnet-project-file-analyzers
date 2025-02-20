namespace DotNetProjectFile.Licensing;

public abstract record SingleLicense : LicenseExpression
{
    public SingleLicense(string identifier, string? baseLicense, ImmutableArray<string> deprecated)
    {
        Expression = identifier;
        BaseLicense = baseLicense;
        Deprecated = deprecated;
        SpdxInfo = Spdx.TryGetLicense(identifier);
    }

    public override string Expression { get; }

    public ImmutableArray<string> Deprecated { get; }

    public string? BaseLicense { get; }

    public SpdxLicenseInfo? SpdxInfo { get; }

    public override bool SpdxCompliant => SpdxInfo is { };
}
