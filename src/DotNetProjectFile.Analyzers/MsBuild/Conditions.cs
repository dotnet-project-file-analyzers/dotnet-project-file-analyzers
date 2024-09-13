namespace DotNetProjectFile.MsBuild;

internal static class Conditions
{
    public static string ToString(Node node) => string.Join(";", node.Conditions());
}
