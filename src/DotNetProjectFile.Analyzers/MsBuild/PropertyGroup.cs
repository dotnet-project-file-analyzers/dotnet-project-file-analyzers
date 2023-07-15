using System.Xml.Linq;

namespace DotNetProjectFile.MsBuild;

public sealed class PropertyGroup : Node
{
    /// <summary>Initializes a new instance of the <see cref="PropertyGroup"/> class.</summary>
    public PropertyGroup(XElement element, Node parent, Project project) : base(element, parent, project)
    {
        TargetFramework = Children<TargetFramework>();
        TargetFrameworks = Children<TargetFrameworks>();
        ImplicitUsings = Children<ImplicitUsings>();
        NuGetAudit = Children<NuGetAudit>();
        OutputType = Children<OutputType>();
        IsPackable = Children<IsPackable>();
    }

    public Nodes<TargetFramework> TargetFramework { get; }

    public Nodes<TargetFrameworks> TargetFrameworks { get; }

    public Nodes<ImplicitUsings> ImplicitUsings { get; }

    public Nodes<NuGetAudit> NuGetAudit { get; }

    public Nodes<OutputType> OutputType { get; }

    public Nodes<IsPackable> IsPackable { get; }
}
