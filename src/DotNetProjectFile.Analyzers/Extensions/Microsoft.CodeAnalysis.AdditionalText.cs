namespace Microsoft.CodeAnalysis;

/// <summary>Extensions on <see cref="AdditionalText"/>.</summary>
public static class AdditionalTextExtensions
{
    extension(AdditionalText text)
    {
        /// <summary>The location of the text.</summary>
        public IOFile Location => IOFile.Parse(text.Path);
    }
}
