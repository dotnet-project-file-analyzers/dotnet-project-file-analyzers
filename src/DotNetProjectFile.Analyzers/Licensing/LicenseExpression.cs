namespace DotNetProjectFile.Licensing;

public abstract record LicenseExpression()
{
    public virtual bool IsKnown => true;

    public bool IsUnknown => !IsKnown;

    public abstract string Expression { get; }

    public abstract bool SpdxCompliant { get; }

    public abstract bool CompatibleWith(LicenseExpression other);

    public bool CompatibleWith(string other)
        => CompatibleWith(Licenses.FromExpression(other));

    public sealed override string ToString()
        => Expression;
}
