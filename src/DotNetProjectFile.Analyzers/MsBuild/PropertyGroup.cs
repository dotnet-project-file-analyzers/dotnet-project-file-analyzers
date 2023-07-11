using System.Xml.Linq;

namespace DotNetProjectFile.MsBuild;

public sealed class PropertyGroup : Node
{
    /// <summary>Initializes a new instance of the <see cref="PropertyGroup"/> class.</summary>
    public PropertyGroup(XElement element, Project project) : base(element, project) { }

    public Nodes<TargetFramework> TargetFramework => Children<TargetFramework>();

    public Nodes<TargetFrameworks> TargetFrameworks => Children<TargetFrameworks>();

    public Nodes<ImplicitUsings> ImplicitUsings => Children<ImplicitUsings>();

    public Nodes<NuGetAudit> NuGetAudit => Children<NuGetAudit>();

    public Nodes<OutputType> OutputType => Children<OutputType>();
}
