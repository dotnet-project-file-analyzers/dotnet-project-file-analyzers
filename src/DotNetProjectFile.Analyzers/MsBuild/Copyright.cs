﻿namespace DotNetProjectFile.MsBuild;

public sealed class Copyright(XElement element, Node parent, Project project)
    : Node<string>(element, parent, project) { }
