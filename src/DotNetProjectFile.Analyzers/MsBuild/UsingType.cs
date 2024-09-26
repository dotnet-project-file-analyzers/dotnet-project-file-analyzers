namespace DotNetProjectFile.MsBuild;

public enum UsingType
{
    Default = 0,
    Static = 1,
    Alias = 2,
    StaticAlias = 3,
}

public static class UsingTypeExtensions
{
    public static string GetPrettyName(this UsingType type)
        => type switch
        {
            UsingType.Static => "Using Static",
            UsingType.Alias => "Using Alias",
            _ => "Using",
        };
}
