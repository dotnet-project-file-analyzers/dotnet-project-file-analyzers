﻿using System.IO;
using System.Xml.Linq;

namespace DotNetProjectFile.Xml;

public sealed class Folder : Node
{
    public Folder(XElement element, Project project) : base(element, project) { }

    public string? Include => GetAttribute();

    public DirectoryInfo? Directory
        => Include is { }
        ? new(Path.Combine(Project.Path.FullName, Include))
        : null;
}
