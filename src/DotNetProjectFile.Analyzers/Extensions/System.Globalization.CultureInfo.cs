namespace System.Globalization;

internal static class CultureInfoExtensions
{
    public static IEnumerable<CultureInfo> Ancestors(this CultureInfo culture)
    {
        var parent = culture.Parent;

        while (parent is { })
        {
            yield return parent;
            parent = parent.Parent;

            if (parent.IsInvariant())
            {
                yield break;
            }
        }
    }

    public static bool IsInvariant(this CultureInfo culture) => culture == CultureInfo.InvariantCulture;
}
