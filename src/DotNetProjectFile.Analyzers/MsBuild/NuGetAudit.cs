﻿namespace DotNetProjectFile.MsBuild;

public sealed class NuGetAudit : Node<bool?>
{
    public NuGetAudit(XElement element, Node parent, Project project) : base(element, parent, project) { }
}
