using DotNetProjectFile.Text;
using System.Collections.Frozen;
using System.Text;

namespace DotNetProjectFile.Licensing;

// Based on https://spdx.org/licenses/

public static class Licenses
{
    /// <summary>An unknown (unresolvable) license.</summary>
    public static readonly LicenseExpression Unknown = UnknownLicense.Instance;

    /// <summary>The permissive MIT license.</summary>
    public static readonly PermissiveLicense MIT = new("MIT");

    // Note that this list is not and will never be legal advice.
    public static readonly ImmutableArray<LicenseExpression> All =
    [
        // TODO: the remainder of the recognized list.
        Unknown,
        MIT,
        new PermissiveLicense("Apache-1.0"),
        new PermissiveLicense("Apache-1.1"),
        new PermissiveLicense("Apache-2.0"),
        new PermissiveLicense("0BSD"),
        new PermissiveLicense("BSD-1-Clause"),
        new PermissiveLicense("BSD-2-Clause"),
        new PermissiveLicense("BSD-3-Clause"),
        new PermissiveLicense("BSD-4-Clause"),
        new PermissiveLicense("Unlicense"),
        new PermissiveLicense("MS-PL"),
        new PermissiveLicense("MS-LPL"),
        new PermissiveLicense("PostgreSQL"),
        new PermissiveLicense("WTFPL"),
        new PermissiveLicense(string.Empty),

        // MPL is technically copy-left, but only on a source-file level, so can be statically and dynamically linked.
        new PermissiveLicense("MPL-1.0"),
        new PermissiveLicense("MPL-1.1"),
        new PermissiveLicense("MPL-2.0"),
        new PermissiveLicense("MPL-2.0-no-copyleft-exception", baseLicense: "MPL-2.0"),

        // LGPL is technically copy-left, but only when linking statically. Since C# code is usually linked dynamically, we allow it for now.
        new PermissiveLicense("LGPL-2.0-only", deprecated: ["LGPL-2.0"]),
        new PermissiveLicense("LGPL-2.0-or-later", deprecated: ["LGPL-2.0+"], baseLicense: "LGPL-2.0-only"),
        new PermissiveLicense("LGPL-2.1-only", deprecated: ["LGPL-2.1"]),
        new PermissiveLicense("LGPL-2.1-or-later", deprecated: ["LGPL-2.1+"], baseLicense: "LGPL-2.1-only"),
        new PermissiveLicense("LGPL-3.0-only", deprecated: ["LGPL-3.0"]),
        new PermissiveLicense("LGPL-3.0-or-later", deprecated: ["LGPL-3.0+"], baseLicense: "LGPL-3.0-only"),

        // EUPL is statically linked copy-left, with even more exceptions than LGPL.
        new PermissiveLicense("EUPL-1.0"),
        new PermissiveLicense("EUPL-1.1"),
        new PermissiveLicense("EUPL-1.2"),

        new PermissiveLicense("OLDAP-1.1"),
        new PermissiveLicense("OLDAP-1.2"),
        new PermissiveLicense("OLDAP-1.3"),
        new PermissiveLicense("OLDAP-1.4"),
        new PermissiveLicense("OLDAP-2.0"),
        new PermissiveLicense("OLDAP-2.0.1"),
        new PermissiveLicense("OLDAP-2.1"),
        new PermissiveLicense("OLDAP-2.2"),
        new PermissiveLicense("OLDAP-2.2.1"),
        new PermissiveLicense("OLDAP-2.2.2"),
        new PermissiveLicense("OLDAP-2.3"),
        new PermissiveLicense("OLDAP-2.4"),
        new PermissiveLicense("OLDAP-2.5"),
        new PermissiveLicense("OLDAP-2.6"),
        new PermissiveLicense("OLDAP-2.7"),
        new PermissiveLicense("OLDAP-2.8"),

        new PermissiveLicense("Zlib"),
        new PermissiveLicense("zlib-acknowledgement", baseLicense: "Zlib"),

        // EPL is similar situation as LGPL
        new PermissiveLicense("EPL-1.0"),
        new PermissiveLicense("EPL-2.0"),

        new CopyLeftLicense("GPL-1.0-only", deprecated: ["GPL-1.0"]),
        new CopyLeftLicense("GPL-2.0-only", deprecated: ["GPL-2.0"]),
        new CopyLeftLicense("GPL-3.0-only", deprecated: ["GPL-3.0"], compatibilities: ["AGPL-3.0-only"]), // AGPL3 allowed due to clause 13 in GPL3

        new CopyLeftLicense("GPL-1.0-or-later", deprecated: ["GPL-1.0+"], compatibilities: ["GPL-1.0-only", "GPL-2.0-only", "GPL-3.0-only", "GPL-2.0-or-later", "GPL-3.0-or-later"], baseLicense: "GPL-1.0-only"),
        new CopyLeftLicense("GPL-2.0-or-later", deprecated: ["GPL-2.0+"], compatibilities: ["GPL-2.0-only", "GPL-3.0-only", "GPL-3.0-or-later"], baseLicense: "GPL-2.0-only"),
        new CopyLeftLicense("GPL-3.0-or-later", deprecated: ["GPL-3.0+"], compatibilities: ["GPL-3.0-only"], baseLicense: "GPL-3.0-only"),

        new CopyLeftLicense("AGPL-1.0-only", deprecated: ["AGPL-1.0"]),
        new CopyLeftLicense("AGPL-3.0-only", deprecated: ["AGPL-3.0"]),

        new CopyLeftLicense("AGPL-1.0-or-later", deprecated: ["AGPL-1.0+"], compatibilities: ["AGPL-1.0-only", "AGPL-3.0-only", "AGPL-3.0-or-later"], baseLicense: "AGPL-1.0-only"),
        new CopyLeftLicense("AGPL-3.0-or-later", deprecated: ["AGPL-3.0+"], compatibilities: ["AGPL-3.0-only"], baseLicense: "AGPL-3.0-only"),

        // Well-known propietary licenses
        new PropietaryLicense("NET_Library_EULA_ENU", compatibleWith: _ => true), // All older versions of .NET Distributable libraries, prior to switching to MIT or Apache 2.0.
    ];

    /// <summary>All permissive licenses.</summary>
    public static readonly ImmutableArray<LicenseExpression> Permissive = [.. All.OfType<PermissiveLicense>()];

    private static readonly FrozenDictionary<string, LicenseExpression> Lookup = CreateLookup();

    private static readonly int LicenseTextLookupNGramSize = 2;
    private static readonly FrozenDictionary<NGramsCollection, SingleLicense> LicenseTextLookup
        = All
        .OfType<SingleLicense>()
        .Where(license => license.BaseLicense is null)
        .SelectMany(license => (license.SpdxInfo?.LicenseTexts ?? []).Select(text => (Text: text, License: license)))
        .ToFrozenDictionary(
            pair => PrepareLicenseText(pair.Text).GetNGrams(LicenseTextLookupNGramSize),
            pair => pair.License);

    private static readonly ImmutableArray<string> GenericLicenseUrlDomains =
    [
        "opensource.org/licenses/",
        "licenses.nuget.org/",
        "spdx.org/licenses/",
    ];

    private static readonly Dictionary<string, string> AdditionalLicenseUrlsRaw = new()
    {
        ["http://go.microsoft.com/fwlink/?LinkId=329770"] = "MIT",
        ["https://github.com/dotnet/corefx/blob/master/LICENSE.TXT"] = "MIT",
        ["https://github.com/dotnet/coreclr/blob/master/LICENSE.TXT"] = "MIT",
        ["https://github.com/dotnet/roslyn/blob/master/License.txt"] = "MIT",
        ["https://github.com/dotnet/core-setup/blob/master/LICENSE.TXT"] = "MIT",
        ["https://github.com/Microsoft/visualfsharp/blob/master/License.txt"] = "MIT",
        ["https://raw.githubusercontent.com/aspnet/AspNetCore/2.0.0/LICENSE.txt"] = "Apache-2.0",
        ["https://raw.githubusercontent.com/aspnet/Home/2.0.0/LICENSE.txt"] = "Apache-2.0",
        ["https://www.gnu.org/licenses/lgpl.html"] = "LGPL-3.0-only",
        ["https://www.gnu.org/licenses/agpl.html"] = "AGPL-3.0-only",
        ["https://www.gnu.org/licenses/gpl.html"] = "GPL-3.0-only",
        ["https://www.gnu.org/licenses/fdl.html"] = "GFDL-1.3-only",
        ["https://www.opensource.org/licenses/bsd-license.php"] = "BSD-2-Clause",
    };

    private static readonly FrozenDictionary<string, LicenseExpression> AdditionalLicenseUrls
        = AdditionalLicenseUrlsRaw
        .ToFrozenDictionary(x => SimplifyUrl(x.Key), x => FromExpression(x.Value), StringComparer.OrdinalIgnoreCase);

    private static readonly FrozenDictionary<string, LicenseExpression> SpdxLicenseUrls
        = All
        .OfType<SingleLicense>()
        .SelectMany(license => (license.SpdxInfo?.SeeAlso ?? []).Select(url => (SimplifyUrl(url), license)))
        .Where(x => x.Item1 is { Length: > 0 })
        .GroupBy(x => x.Item1)
        .Select(x => x.MinBy(static x => x.license.Expression.Length))
        .ToFrozenDictionary(x => x.Item1, x => x.license as LicenseExpression, StringComparer.OrdinalIgnoreCase);

    private static FrozenDictionary<string, LicenseExpression> CreateLookup()
    {
        var result = new Dictionary<string, LicenseExpression>();

        foreach (var license in All)
        {
            result[license.Expression] = license;

            if (license is SingleLicense s)
            {
                foreach (var d in s.Deprecated)
                {
                    result[d] = license;
                }
            }
        }

        return result.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
    }

    public static LicenseExpression FromExpression(string? licenseExpression)
    {
        if (licenseExpression is not { Length: > 0 })
        {
            return Unknown;
        }

        // TODO: handle complex expressions: https://spdx.github.io/spdx-spec/v2-draft/using-SPDX-short-identifiers-in-source-files/#e4-representing-multiple-licenses

        if (Lookup.TryGetValue(licenseExpression, out var result))
        {
            return result;
        }
        else
        {
            return Unknown;
        }
    }

    /// <summary>Tries to resolve a license from the license URL.</summary>
    /// <remarks>
    /// TODO see-also entries from https://github.com/spdx/license-list-data/blob/main/json/licenses.json.
    /// </remarks>
    public static LicenseExpression FromUrl(string? licenseUrl)
    {
        var url = SimplifyUrl(licenseUrl);

        return url switch
        {
            null or "" => Unknown,
            _ when AdditionalLicenseUrls.TryGetValue(url, out var additional) => additional,
            _ when SpdxLicenseUrls.TryGetValue(url, out var spdx) => spdx,
            _ when url.EndsWith(".mit-license.org") => MIT,
            _ when TryGenericLicenseUrlDomains(url) is { } generic => generic,
            _ => Unknown,
        };

        LicenseExpression? TryGenericLicenseUrlDomains(string url)
            => GenericLicenseUrlDomains
            .Select(url.TrimStart)
            .Where(trimmed => trimmed != url)
            .Select(FromExpression)
            .FirstOrDefault();
    }

    [return: NotNullIfNotNull(nameof(url))]
    private static string? SimplifyUrl(string? url) => url?
        .TrimStart("https://")
        .TrimStart("http://")
        .TrimStart("www.")
        .TrimEnd('/')
        .TrimEnd(".php")
        .TrimEnd(".html")
        .TrimEnd(".htm")
        .TrimEnd(".en")
        .TrimEnd("-license");

    private static string PrepareLicenseText(string text)
    {
        var sb = new StringBuilder(text.Length);
        foreach (var c in text.Where(char.IsLetter))
        {
            sb.Append(char.ToLowerInvariant(c));
        }
        return sb.ToString();
    }

    public static LicenseExpression FromFile(IOFile? licenseFile)
    {
        if (licenseFile is not { Exists: true } file)
        {
            return Unknown;
        }

        var contentRaw = file.ReadAllText();
        var content = PrepareLicenseText(contentRaw);
        var contentNgrams = content.GetNGrams(LicenseTextLookupNGramSize);

        // Matching logic is loosely based on the logic used by Licensee: https://github.com/licensee/licensee
        // Licensee is the library used by GitHub for determining the license.

        foreach (var pair in LicenseTextLookup)
        {
            var modelNgrams = pair.Key;

            if (contentNgrams.DiceSorensenCoefficientAtLeast(modelNgrams, 0.95f))
            {
                return pair.Value;
            }
        }

        return CustomLicense.Create(content);
    }
}
