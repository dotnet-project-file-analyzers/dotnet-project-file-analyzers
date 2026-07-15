namespace System.Globalization;

internal static class CultureInfoExtensions
{
    extension(CultureInfo culture)
    {
        public IEnumerable<CultureInfo> Ancestors()
        {
            var parent = culture.Parent;

            while (!parent.IsInvariant)
            {
                yield return parent;
                parent = parent.Parent;
            }
            yield return CultureInfo.InvariantCulture;
        }

        public bool IsInvariant => culture == CultureInfo.InvariantCulture;
    }
}
