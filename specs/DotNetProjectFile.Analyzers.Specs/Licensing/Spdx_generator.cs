#pragma warning disable RS1035 // FP

using DotNetProjectFile.Licensing;
using DotNetProjectFile.NuGet.Packaging;
using DotNetProjectFile.NuGet;
using System.Collections.Immutable;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

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
        var requestedLicenses = list?.Licenses?.OfType<License>() ?? [];
        var manualLicenses = GetAdditionalLicenses();
        var licenses = requestedLicenses.Concat(manualLicenses);
        var valid = licenses
            .Where(l => l.LicenseId is { Length: > 0 })
            .Where(l => !l.IsDeprecatedLicenseId)
            .Where(l => Licenses.FromExpression(l.LicenseId) != Licenses.Unknown);

        var curDir = GetCurrentDirectoryPath();
        var outputDir = Path.Combine(curDir, "../../../src/DotNetProjectFile.Analyzers/Licensing/Generated");
        Directory.CreateDirectory(outputDir);
        var outputFile = Path.Combine(outputDir, "spdx_info.bin");

        var infos = new List<SpdxLicenseInfo>();
        foreach (var license in valid.OrderBy(x => x.LicenseId))
        {
            var info = new SpdxLicenseInfo()
            {
                Id = license.LicenseId!,
                Name = license.Name ?? license.LicenseId!,
                Fsf = license.IsFsfLibre,
                Osi = license.IsOsiApproved,
                SeeAlso = license.SeeAlso?.OfType<string>().ToImmutableArray() ?? [],
                LicenseText = (await GetLicenseText(license.LicenseId!))?.Trim().Replace("\r\n", "\n"),
            };
            infos.Add(info);
        }

        // Encode and compress spdx data.
        using var compressed = new MemoryStream();
        SpdxLicenseInfo.WriteAllToCompressed(infos, compressed);

        // Validate we can read the stream back.
        compressed.Position = 0;
        var readBack = SpdxLicenseInfo.ReadAllFromCompressed(compressed);
        readBack.Should().BeEquivalentTo(infos);

        // Save compressed data.
        compressed.Position = 0;
        using var fs = File.Create(outputFile);
        await compressed.CopyToAsync(fs);
        await fs.FlushAsync();

        async Task<string?> GetLicenseText(string id)
        {
            var dirManual = Path.Combine(GetCurrentDirectoryPath(), "LicenseTexts/Manual");
            var dirGenerated = Path.Combine(GetCurrentDirectoryPath(), "LicenseTexts/Generated");
            Directory.CreateDirectory(dirManual);
            Directory.CreateDirectory(dirGenerated);

            var manualFileName = Path.Combine(dirManual, $"{id}.txt");
            var generatedFileName = Path.Combine(dirGenerated, $"{id}.txt");

            try
            {
                if (File.Exists(manualFileName))
                {
                    var fromFile = await File.ReadAllTextAsync(manualFileName);
                    if (string.IsNullOrWhiteSpace(fromFile))
                    {
                        return null;
                    }

                    return fromFile;
                }

                if (File.Exists(generatedFileName))
                {
                    var fromFile = await File.ReadAllTextAsync(generatedFileName);
                    if (string.IsNullOrWhiteSpace(fromFile))
                    {
                        return null;
                    }

                    return fromFile;
                }

                using var detailsResponse = await client.GetAsync($"{SpdxUrl}{id}.json");
                var details = await detailsResponse.Content.ReadFromJsonAsync<LicenseDetails>();
                var text = details?.LicenseText;

                await File.WriteAllTextAsync(generatedFileName, text ?? string.Empty);

                return text;
            }
            catch
            {
                if (!File.Exists(generatedFileName))
                {
                    try
                    {
                        await File.WriteAllTextAsync(generatedFileName, "");
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

        IEnumerable<License> GetAdditionalLicenses()
        {
            yield return new License
            {
                LicenseId = "NET_Library_EULA_ENU",
                Name = ".NET Library License Terms",
                SeeAlso =
                [
                    "https://www.microsoft.com/web/webpi/eula/net_library_eula_enu.htm",
                    "https://dotnet.microsoft.com/en-us/dotnet_library_license.htm",
                    "https://go.microsoft.com/fwlink/?LinkId=529443",
                    "https://www.microsoft.com/web/webpi/eula/net_library_eula_enu.htm",
                    "https://go.microsoft.com/fwlink/?LinkID=320539",
                ],
            };
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
