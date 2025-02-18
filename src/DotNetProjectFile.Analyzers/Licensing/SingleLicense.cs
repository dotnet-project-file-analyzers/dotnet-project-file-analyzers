namespace DotNetProjectFile.Licensing;

public abstract record SingleLicense : LicenseExpression
{
    public SingleLicense(string identifier, ImmutableArray<string> deprecated)
    {
        Expression = identifier;
        Deprecated = deprecated;
        SpdxInfo = Spdx.TryGetLicense(identifier);
    }

    public override string Expression { get; }

    public ImmutableArray<string> Deprecated { get; }

    public SpdxLicenseInfo? SpdxInfo { get; }
}
