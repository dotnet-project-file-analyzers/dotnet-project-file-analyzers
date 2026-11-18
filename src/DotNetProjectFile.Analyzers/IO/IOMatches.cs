namespace DotNetProjectFile.IO;

public static class IOMatches
{
    [DebuggerDisplay("Local is {Value}")]
    public sealed class LocalIs(string value) : IOMatch
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string Value = value;

        /// <inheritdoc />
        [Pure]
        public bool Matches(Chars full, Chars local) => local.Equals(Value, StringComparison.Ordinal);
    }

    [DebuggerDisplay("Ends with {Value}")]
    public sealed class EndsWith(string value) : IOMatch
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string Value = value;

        /// <inheritdoc />
        [Pure]
        public bool Matches(Chars full, Chars local) => full.EndsWith(Value, StringComparison.Ordinal);
    }
}
