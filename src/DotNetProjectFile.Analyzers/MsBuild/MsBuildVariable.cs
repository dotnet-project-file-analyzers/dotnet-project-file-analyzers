using System.Text.RegularExpressions;

namespace DotNetProjectFile.MsBuild;

public readonly struct MsBuildVariable
{
    public static readonly MsBuildVariable Empty;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string Name;

    private MsBuildVariable(string name) => Name = name;

    public bool HasValue => Name is not null;

    /// <inheritdoc />
    public override string ToString() => Name is null ? string.Empty : $"$({Name})";

    public static MsBuildVariable TryParse(string str)
        => Pattern.Match(str) is { Success: true } match && match.Length == str.Trim().Length
        ? new(match.Groups[nameof(Name)].Value)
        : Empty;

    public static IEnumerable<MsBuildVariable> ParseAll(string str)
        => ((IEnumerable<Match>)Pattern.Matches(str))
        .Select(m => new MsBuildVariable(m.Groups[nameof(Name)].Value));


    private static readonly Regex Pattern = new(@"\$\((?<Name>[A-Z]+)\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
}
