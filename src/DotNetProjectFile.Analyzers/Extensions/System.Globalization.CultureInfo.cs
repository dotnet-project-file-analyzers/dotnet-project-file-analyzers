namespace System.Globalization;

internal static class CultureInfoExtensions
{
    public static IEnumerable<CultureInfo> Ancestors(this CultureInfo culture)
    {
        var parent = culture.Parent;

        while (!parent.IsInvariant())
        {
            yield return parent;
            parent = parent.Parent;
        }
        yield return CultureInfo.InvariantCulture;
    }

    public static bool IsInvariant(this CultureInfo culture) => culture == CultureInfo.InvariantCulture;
}
