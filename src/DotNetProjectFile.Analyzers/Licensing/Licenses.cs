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
        new PermissiveLicense("MPL-2.0-no-copyleft-exception"),

        new CopyLeftLicense("GPL-1.0-only", deprecated: ["GPL-1.0"]),
        new CopyLeftLicense("GPL-2.0-only", deprecated: ["GPL-2.0"]),
        new CopyLeftLicense("GPL-3.0-only", deprecated: ["GPL-3.0"], compatibilities: ["AGPL-3.0-only"]), // AGPL3 allowed due to clause 13 in GPL3

        new CopyLeftLicense("GPL-1.0-or-later", deprecated: ["GPL-1.0+"], compatibilities: ["GPL-1.0-only", "GPL-2.0-only", "GPL-3.0-only", "GPL-2.0-or-later", "GPL-3.0-or-later"]),
        new CopyLeftLicense("GPL-2.0-or-later", deprecated: ["GPL-2.0+"], compatibilities: ["GPL-2.0-only", "GPL-3.0-only", "GPL-3.0-or-later"]),
        new CopyLeftLicense("GPL-3.0-or-later", deprecated: ["GPL-3.0+"], compatibilities: ["GPL-3.0-only"]),

        new CopyLeftLicense("AGPL-1.0-only", deprecated: ["AGPL-1.0"]),
        new CopyLeftLicense("AGPL-3.0-only", deprecated: ["AGPL-3.0"]),

        new CopyLeftLicense("AGPL-1.0-or-later", deprecated: ["AGPL-1.0+"], compatibilities: ["AGPL-1.0-only", "AGPL-3.0-only", "AGPL-3.0-or-later"]),
        new CopyLeftLicense("AGPL-3.0-or-later", deprecated: ["AGPL-3.0+"], compatibilities: ["AGPL-3.0-only"]),
    ];

    private static readonly FrozenDictionary<string, LicenseExpression> Lookup = CreateLookup();

    private static readonly ImmutableArray<string> GenericLicenseUrlDomains =
    [
        "opensource.org/licenses",
        "licenses.nuget.org",
        "spdx.org/licenses",
    ];

    private static readonly string GenericLicenseUrlDomainPattern
        = string.Join("|", GenericLicenseUrlDomains.Select(d => $"({Regex.Escape(d)})"));

    private static readonly Regex GenericLicenseUrlRegex = new(@$"(https?:\/\/)?(www\.)?({GenericLicenseUrlDomainPattern})\/(?<exp>[a-zA-Z0-9-.]+)(\.html)?\/?", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

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

        return result.ToFrozenDictionary();
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

        if (GenericLicenseUrlRegex.Match(licenseUrl) is { Success: true } match)
        {
            var expression = match.Groups["exp"].Value;
            return FromExpression(expression);
        }

        // TODO see-also entries from https://github.com/spdx/license-list-data/blob/main/json/licenses.json
        return Unknown;
    }

    public static LicenseExpression FromFile(string? licenseUrl)
    {
        // TODO: fuzzy match the file content to known license texts
        return Unknown;
    }
}
