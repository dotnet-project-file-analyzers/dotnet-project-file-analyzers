namespace Microsoft.CodeAnalysis;

public static class AdditionalFileAnalysisContextExtensions
{
    extension(AdditionalFileAnalysisContext context)
    {
        /// <summary>
        /// Gets the <see cref="AnalyzerType"/> as set in the metadata.
        /// </summary>
        public AnalyzerType AnalyzerType
            => context.Options.AnalyzerConfigOptionsProvider
                .GetOptions(context.AdditionalFile)
                .TryGetValue("build_metadata.AdditionalFiles.AnalyzerType", out var value)
            && Enum.TryParse<AnalyzerType>(value, ignoreCase: true, out var type)
                ? type
                : AnalyzerType.None;

        /// <summary>
        /// Applies if the analyzer type matches; or has not been set and the
        /// predicate is true.
        /// </summary>
        public IOFile? Applies(Func<IOFile, bool> predicate, params AnalyzerType[] types)
        {
            var file = IOFile.Parse(context.AdditionalFile.Path);

            return context.AnalyzerType switch
            {
                var type when types.Contains(type) => file,
                AnalyzerType.None when predicate(file) => file,
                _ => null,
            };
        }
    }
}
