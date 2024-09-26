using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.CodeAnalysis;

public static class ProjectFileExtensions
{
    [Pure]
    public static Location GetLocation(this ProjectFile file, LinePositionSpan span)
        => Location.Create(file.Path.ToString(), file.Text.TextSpan(span), span);

    [Pure]
    public static bool IsAdditional(this ProjectFile file, IEnumerable<AdditionalText> texts) => texts
        .Select(f => IOFile.Parse(f.Path))
        .Any(f => f.Equals(file.Path));
}
