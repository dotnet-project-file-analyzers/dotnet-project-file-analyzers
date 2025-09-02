namespace DotNetProjectFile.MsBuild;

public static class OutputTypeKindExtensions
{
    /// <summary>Indicates whether the <see cref="OutputType.Kind"/> is an executable.</summary>
    [Pure]
    public static bool IsExe(this OutputType.Kind kind) => kind
        is OutputType.Kind.Exe
        or OutputType.Kind.WinExe
        or OutputType.Kind.AppContainerExe;
}
