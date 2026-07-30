using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis;

/// <summary>Extensions on <see cref="AdditionalText"/>.</summary>
public static class AdditionalTextExtensions
{
    extension(AdditionalText additionText)
    {
        /// <summary>The location of the text.</summary>
        public IOFile Location => IOFile.Parse(additionText.Path);

        /// <inheritdoc cref="AdditionalText.GetText(CancellationToken)" />
        /// <exception cref="InvalidOperationException">
        /// When the <see cref="SourceText"/> could not be resolved.
        /// </exception>
        [Pure]
        public SourceText Text()
            => additionText.GetText()
            ?? throw new InvalidOperationException($"Could not get source text for {additionText.Location}.");
    }
}
