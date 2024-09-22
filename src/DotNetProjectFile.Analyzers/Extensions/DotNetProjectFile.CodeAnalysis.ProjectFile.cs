using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.CodeAnalysis;

public static class ProjectFileExtensions
{
    [Pure]
    public static Location GetLocation(this ProjectFile file, LinePositionSpan span)
        => Location.Create(file.Path.ToString(), file.Text.TextSpan(span), span);
}
