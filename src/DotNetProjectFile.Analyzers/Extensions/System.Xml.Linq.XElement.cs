namespace System.Xml.Linq;

internal static class XElementExtensions
{
    public static bool IsSelfClosing(this XElement element)
    {
        if (element.Value is not { Length: > 0 })
        {
            return element.ToString()[^2] == '/';
        }
        else
        {
            return false;
        }
    }
}
