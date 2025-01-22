using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetProjectFile.Ini;

public sealed class KeySyntax(IniParser.KeyContext context) : IniSyntax(context)
{
}
