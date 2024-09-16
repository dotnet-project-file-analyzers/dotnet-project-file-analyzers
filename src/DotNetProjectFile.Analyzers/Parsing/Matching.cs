namespace DotNetProjectFile.Parsing;

/// <summary>Matching results.</summary>
public enum Matching
{
    /// <summary>No match.</summary>
    NoMatch = -1,

    /// <summary>End of File (EoF).</summary>
    EoF = 0,

    /// <summary>Match.</summary>
    Match = 1,
}
