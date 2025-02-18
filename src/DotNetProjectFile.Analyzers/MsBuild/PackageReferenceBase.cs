using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetProjectFile.MsBuild;

public abstract class PackageReferenceBase(XElement element, Node parent, MsBuildProject project)
    : Node(element, parent, project)
{
    public string? Include => Attribute();

    public string? Update => Attribute();

    public string? Version => Attribute();

    public string IncludeOrUpdate => Include ?? Update ?? string.Empty;
}
