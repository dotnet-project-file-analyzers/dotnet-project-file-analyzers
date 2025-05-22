using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Slnx;

public sealed class Solution(XElement element, Node parent)
    : Node(element, parent, null), ProjectFile
{
    public IOFile Path => throw new NotImplementedException();

    public SourceText Text => throw new NotImplementedException();

    public WarningPragmas WarningPragmas => throw new NotImplementedException();
}
