namespace DotNetProjectFile.RuleCatalog;

/// <summary>
/// Collects <see cref="DiagnosticInfo"/> from <see cref="NugetPackage"/>s.
/// </summary>
public static class DiagnosticCollector
{
    /// <summary>
    /// Checks for new versions of <see cref="NugetPackage"/>s and updates their rules accordingaly.
    /// </summary>
    /// <param name="collection">
    /// The collection to enrich.
    /// </param>
    /// <param name="cancellation">
    /// A cancellation token.
    /// </param>
    /// <returns>
    /// The enriched collection.
    /// </returns>
    [Pure]
    public static async Task<DiagnosticCollection> Collect(DiagnosticCollection collection, CancellationToken cancellation = default)
    {
        var packages = new Dictionary<string, NugetPackage>();

        foreach (var package in collection.Packages)
        {
            var rules = packages.TryGetValue(package.Id, out var existing)
                ? existing.Rules.ToDictionary(r => r.Id, r => r)
                : [];

            foreach (var rule in package.Rules)
                rules[rule.Id] = rule;

            var versions = await NuGetRepository.NewVersions(package, cancellation);
            var updated = package;

            foreach (var version in versions)
            {
                updated = updated with { Version = version };

                var updates = await NuGetRepository.Diagnostics(updated, cancellation);

                foreach (var update in updates)
                {
                    rules[update.Id] = rules.TryGetValue(update.Id, out var prev)
                        ? prev.Update(update)
                        : update;
                }
            }

            packages[package.Id] = updated with { Rules = [.. rules.Values.Order()] };
        }

        return collection with { Packages = [.. packages.Values.Order()] };
    }
}
