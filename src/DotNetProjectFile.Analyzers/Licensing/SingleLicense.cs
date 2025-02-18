namespace DotNetProjectFile.Licensing;

public abstract record SingleLicense(string Identifier, ImmutableArray<string> Deprecated) : LicenseExpression()
{
    public override string Expression => Identifier;
}
