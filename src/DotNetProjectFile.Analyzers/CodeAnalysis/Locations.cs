using DotNetProjectFile.Resx;
using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.CodeAnalysis;

public static class Locations
{
    public static Location GetLocation(this MsBuildProject project, LinePositionSpan span)
        => Location.Create(project.Path.ToString(), project.Text.TextSpan(span), span);

    public static Location GetLocation(this Resource resource, LinePositionSpan span)
        => Location.Create(resource.Path.ToString(), resource.Text.TextSpan(span), span);
}
