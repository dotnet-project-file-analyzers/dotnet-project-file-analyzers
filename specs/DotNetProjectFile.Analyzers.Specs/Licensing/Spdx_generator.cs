#pragma warning disable RS1035 // FP

using DotNetProjectFile.Licensing;
using System.Collections.Immutable;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Web;

namespace Licensing.Spdx;

public class Generator
{
    private static readonly string SpdxUrl = "https://spdx.org/licenses/";
    private static readonly string SpdxLicensesUrl = $"{SpdxUrl}licenses.json";

    [Category("Generators")]
    [Test]
    public async Task Generate()
    {
        using var client = new HttpClient();
        using var response = await client.GetAsync(SpdxLicensesUrl);
        response.IsSuccessStatusCode.Should().BeTrue();

        var list = await response.Content.ReadFromJsonAsync<LicenseList>();
        var licenses = list?.Licenses?.OfType<License>() ?? [];
        var valid = licenses
            .Where(l => l.LicenseId is { Length: > 0 })
            .Where(l => !l.IsDeprecatedLicenseId)
            .Where(l => Licenses.FromExpression(l.LicenseId) != Licenses.Unknown);

        var curDir = GetCurrentDirectoryPath();
        var outputDir = Path.Combine(curDir, "../../../src/DotNetProjectFile.Analyzers/Licensing/Generated");
        Directory.CreateDirectory(outputDir);

        var infos = new List<SpdxLicenseInfo>();
        foreach (var license in valid)
        {
            var info = new SpdxLicenseInfo()
            {
                Id = license.LicenseId!,
                Name = license.Name ?? license.LicenseId!,
                Fsf = license.IsFsfLibre,
                Osi = license.IsOsiApproved,
                SeeAlso = license.SeeAlso?.OfType<string>().ToImmutableArray() ?? [],
                LicenseText = await GetLicenseText(license.LicenseId!),
            };
            infos.Add(info);
        }

        Assert.Fail();

        async Task<string?> GetLicenseText(string id)
        {
            var dir = Path.Combine(GetCurrentDirectoryPath(), "Generated");
            Directory.CreateDirectory(dir);

            var fileName = Path.Combine(dir, $"{id}.txt");

            try
            {
                if (File.Exists(fileName))
                {
                    var fromFile = await File.ReadAllTextAsync(fileName);
                    if (string.IsNullOrWhiteSpace(fromFile))
                    {
                        return null;
                    }
                }

                using var detailsResponse = await client.GetAsync($"{SpdxUrl}{id}.json");
                var details = await detailsResponse.Content.ReadFromJsonAsync<LicenseDetails>();
                var text = details?.LicenseText;

                await File.WriteAllTextAsync(fileName, text ?? string.Empty);

                return text;
            }
            catch
            {
                if (!File.Exists(fileName))
                {
                    try
                    {
                        await File.WriteAllTextAsync(fileName, "");
                    }
                    catch
                    {
                        // Do nothing.
                    }
                }

                return null;
            }
        }

        string GetCurrentDirectoryPath([CallerFilePath] string? path = null)
        {
            if (path is { Length: > 0 })
            {
                return Path.GetDirectoryName(path) ?? Directory.GetCurrentDirectory();
            }

            return Directory.GetCurrentDirectory();
        }
    }

    public sealed record LicenseList
    {
        public string? LicenseListVersion { get; init; }

        public License?[]? Licenses { get; init; }

        public string? ReleaseDate { get; init; }
    }

    public record License
    {
        public string? Reference { get; init; }

        public bool IsDeprecatedLicenseId { get; init; }

        public string? DetailsUrl { get; init; }

        public int ReferenceNumber { get; init; }

        public string? Name { get; init; }

        public string? LicenseId { get; init; }

        public string?[]? SeeAlso { get; init; }

        public bool IsOsiApproved { get; init; }

        public bool IsFsfLibre { get; init; }
    }

    public sealed record LicenseDetails
    {
        public string? LicenseText { get; init; }
    }
}
