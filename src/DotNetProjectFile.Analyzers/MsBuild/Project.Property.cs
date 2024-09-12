using DotNetProjectFile.Navigation;

namespace DotNetProjectFile.MsBuild;

public sealed partial class Project
{
    public bool IsPackable()
         => Property<IsPackable>()?.Value ?? MsBuildDefaults.IsPackable;

    public bool IsPublishable()
        => Property<IsPublishable>()?.Value ?? MsBuildDefaults.IsPublishable;

    public bool IsTestProject()
        => Property<IsTestProject>()?.Value ?? MsBuildDefaults.IsTestProject;

    public bool NETAnalyzersEnabled()
        => Property<EnableNETAnalyzers>()?.Value ?? MsBuildDefaults.EnableNETAnalyzers;

    public bool? NuGetAuditEnabled()
        => Property<NuGetAudit>()?.Value ?? MsBuildDefaults.NuGetAudit;

    public bool PackageValidationEnabled()
        => Property<EnablePackageValidation>()?.Value ?? MsBuildDefaults.EnablePackageValidation;

    public bool IsDevelopmentDependency()
        => Property<DevelopmentDependency>()?.Value ?? MsBuildDefaults.DevelopmentDependency;

    public bool? ManagePackageVersionsCentrally()
       => Property<ManagePackageVersionsCentrally>()?.Value ?? MsBuildDefaults.ManagePackageVersionsCentrally;

    public TNode? Property<TNode>() where TNode : Node => this
        .SelftAndDirectoryProps()
        .Select(p => Read<TNode>(p, new(p.Path)))
        .OfType<TNode>()
        .FirstOrDefault();

    /// <remarks>
    /// Walk in reversed order true the nodes.
    /// Skip ItemGroups as they can not contain properties.
    /// </remarks>
    private static TNode? Read<TNode>(Node node, ProjectTrace trace) where TNode : Node
    {
        if (node is TNode typed)
        {
            return typed;
        }
        else if (node is Import import
            && import.Value is { } imported
            && !trace.Contains(imported.Path)
            && Read<TNode>(imported, trace) is TNode value)
        {
            return value;
        }
        else if (node is not ItemGroup)
        {
            for (var i = node.Children.Count - 1; i >= 0; i--)
            {
                if (Read<TNode>(node.Children[i], trace) is TNode child)
                {
                    return child;
                }
            }
        }
        return null;
    }
}
