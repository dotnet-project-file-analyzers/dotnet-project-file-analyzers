namespace System;

internal static class StringExtensions
{
    extension(string? s)
    {
        public string? NullIfEmpty() => s is { Length: > 0 } ? s : null;
    }
}
