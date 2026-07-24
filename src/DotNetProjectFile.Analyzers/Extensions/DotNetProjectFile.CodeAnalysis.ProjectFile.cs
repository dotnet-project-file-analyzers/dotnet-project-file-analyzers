using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.CodeAnalysis;

public static class ProjectFileExtensions
{
    extension(ProjectFile file)
    {
        [Pure]
        public Location GetLocation(LinePositionSpan span)
            => Location.Create(file.Path.ToString(), file.Text.TextSpan(span), span);

        [Pure]
        public bool IsAdditional(IEnumerable<AdditionalText> texts) => texts
            .Any(t => t.Location.Equals(file.Path));
    }
}
