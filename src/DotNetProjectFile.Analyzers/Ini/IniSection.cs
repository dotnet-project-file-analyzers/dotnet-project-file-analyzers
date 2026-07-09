using DotNetProjectFile.Collections;
using Grammr;

namespace DotNetProjectFile.Ini;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public sealed class IniSection(SliceSpan span, GrammrTree tree) : GrammrNode(span, tree)
{
    public IniHeader? Header => field ??= Children.OfType<IniHeader>().SingleOrDefault();

    public GrammrNodes<IniEntry> Entries => new(Children);

    public IEnumerable<KeyValuePair<string, string?>> Kvps => Entries.Where(e => e is { Key: not null })
        .Select(e => new KeyValuePair<string, string?>(e.Key!.Text, e.Value?.Text));

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay
        => Header is null
        ? $"Header = <null>, Entries = {Children.Count}"
        : $"Header = [{Header.Text}], Entries = {Children.Count}";
}
