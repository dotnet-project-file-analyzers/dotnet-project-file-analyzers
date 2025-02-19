using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetProjectFile.Licensing;

// Based on https://spdx.org/licenses/

public static class Licenses
{
    public static readonly LicenseExpression Unknown = UnknownLicense.Instance;

    // Note that this list is not and will never be legal advice.
    public static readonly ImmutableArray<LicenseExpression> All =
    [
        // TODO: the remainder of the recognized list.

        Unknown,
        new PermissiveLicense("MIT"),
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

        // MPL is technically copy-left, but only on a source-file level, so can be statically and dynamically linked.
        new PermissiveLicense("MPL-1.0"),
        new PermissiveLicense("MPL-1.1"),
        new PermissiveLicense("MPL-2.0"),
        new PermissiveLicense("MPL-2.0-no-copyleft-exception", baseLicense: "MPL-2.0"),

        // LGPL is technically copy-left, but only when linking statically. Since C# code is usually linked dynamically, we allow it for now.
        new PermissiveLicense("LGPL-2.0-only", deprecated: ["LGPL-2.0"]),
        new PermissiveLicense("LGPL-2.0-or-later", deprecated: ["LGPL-2.0+"], baseLicense: "LGPL-2.0-only"),
        new PermissiveLicense("LGPL-3.0-only", deprecated: ["LGPL-3.0"]),
        new PermissiveLicense("LGPL-3.0-or-later", deprecated: ["LGPL-3.0+"], baseLicense: "LGPL-3.0-only"),

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
    ];

    /// <summary>All permissive licenses.</summary>
    public static readonly ImmutableArray<LicenseExpression> Permissive = [.. All.OfType<PermissiveLicense>()];

    private static readonly FrozenDictionary<string, LicenseExpression> Lookup = CreateLookup();

    private static readonly ImmutableArray<string> GenericLicenseUrlDomains =
    [
        "opensource.org/licenses/",
        "licenses.nuget.org/",
        "spdx.org/licenses/",
    ];

    private static readonly Dictionary<string, string> AdditionalLicenseUrlsRaw = new()
    {
        ["https://ianhammondcooper.mit-license.org/"] = "MIT",
        ["https://microsoft.mit-license.org/"] = "MIT",
        ["http://go.microsoft.com/fwlink/?LinkId=329770"] = "MIT",
        ["https://github.com/dotnet/corefx/blob/master/LICENSE.TXT"] = "MIT",
        ["https://github.com/dotnet/coreclr/blob/master/LICENSE.TXT"] = "MIT",
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

    public static LicenseExpression FromUrl(string? licenseUrl)
    {
        if (licenseUrl is not { Length: > 0 })
        {
            return Unknown;
        }

        var simplified = SimplifyUrl(licenseUrl);

        if (AdditionalLicenseUrls.TryGetValue(simplified, out var result))
        {
            return result;
        }

        if (SpdxLicenseUrls.TryGetValue(simplified, out result))
        {
            return result;
        }

        foreach (var url in GenericLicenseUrlDomains)
        {
            var tail = simplified.TrimStart(url);
            if (tail != simplified)
            {
                return FromExpression(tail);
            }
        }

        // TODO see-also entries from https://github.com/spdx/license-list-data/blob/main/json/licenses.json
        return Unknown;
    }

    [return: NotNullIfNotNull(nameof(url))]
    private static string? SimplifyUrl(string? url)
    {
        if (url is null)
        {
            return null;
        }

        return url
            .TrimStart("https://")
            .TrimStart("http://")
            .TrimStart("www.")
            .TrimEnd('/')
            .TrimEnd(".php")
            .TrimEnd(".html")
            .TrimEnd(".en")
            .TrimEnd("-license");
    }

    public static LicenseExpression FromFile(string? licenseFile)
    {
        // TODO: fuzzy match the file content to known license texts
        return Unknown;
    }
}
