namespace System;

internal static class ReadOnlySpanExtensions
{
    extension(Chars span)
    {
        /// <summary>Indicates End of Stream/Span.</summary>
        public bool EOS => span.Length is 0;
    }
}
