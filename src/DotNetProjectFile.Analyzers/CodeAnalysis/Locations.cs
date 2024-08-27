using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.CodeAnalysis;

public static class Locations
{
    public static Location GetLocation(this MsBuildProject project, LinePositionSpan span)
        => Location.Create(project.Path.ToString(), project.Text.TextSpan(span), span);
}
