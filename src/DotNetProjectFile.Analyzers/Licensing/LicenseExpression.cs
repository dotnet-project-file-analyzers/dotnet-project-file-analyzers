namespace DotNetProjectFile.Licensing;

public abstract record LicenseExpression()
{
    public virtual bool IsKnown => true;

    public bool IsUnknown => !IsKnown;

    public abstract string Expression { get; }

    public abstract bool SpdxCompliant { get; }

    public abstract bool CompatibleWith(LicenseExpression other);

    public sealed override string ToString()
        => Expression;
}

public static class LicenseExpressionExtensions
{
    public static bool CompatibleWith(this LicenseExpression license, string other)
        => license.CompatibleWith(Licenses.FromExpression(other));
}
