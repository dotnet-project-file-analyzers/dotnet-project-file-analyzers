using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Text;

namespace DotNetProjectFile.NuGet;

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
        new CopyLeftLicense("GPL-3.0-only", deprecated: ["GPL-3.0"]),

        new CopyLeftLicense("GPL-1.0-or-later", deprecated: ["GPL-1.0+"], compatibilities: ["GPL-1.0-only", "GPL-2.0-only", "GPL-3.0-only", "GPL-2.0-or-later", "GPL-3.0-or-later"]),
        new CopyLeftLicense("GPL-2.0-or-later", deprecated: ["GPL-2.0+"], compatibilities: ["GPL-2.0-only", "GPL-3.0-only", "GPL-3.0-or-later"]),
        new CopyLeftLicense("GPL-3.0-or-later", deprecated: ["GPL-3.0+"], compatibilities: ["GPL-3.0-only"]),

        new CopyLeftLicense("AGPL-1.0-only", deprecated: ["AGPL-1.0"]),
        new CopyLeftLicense("AGPL-3.0-only", deprecated: ["AGPL-3.0"]),

        new CopyLeftLicense("AGPL-1.0-or-later", deprecated: ["AGPL-1.0+"], compatibilities: ["AGPL-1.0-only", "AGPL-3.0-only", "AGPL-3.0-or-later"]),
        new CopyLeftLicense("AGPL-3.0-or-later", deprecated: ["AGPL-3.0+"], compatibilities: ["AGPL-3.0-only"]),
    ];

    private static readonly FrozenDictionary<string, LicenseExpression> Lookup = CreateLookup();

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
        // TODO
        return Unknown;
    }

    public static LicenseExpression FromFile(string? licenseUrl)
    {
        // TODO: fuzzy match the file content to known license texts
        return Unknown;
    }
}

public abstract record LicenseExpression()
{
    public abstract string Expression { get; }

    public abstract bool CompatibleWith(LicenseExpression other);

    public sealed override string ToString()
        => Expression;
}

public static class LicenseExpressionExtensions
{
    public static bool CompatibleWith(this LicenseExpression license, string other)
        => license.CompatibleWith(Licenses.FromExpression(other));
}

public sealed record UnknownLicense : LicenseExpression
{
    public static readonly UnknownLicense Instance = new();

    private UnknownLicense()
    {
    }

    public override string Expression => string.Empty;

    public override bool CompatibleWith(LicenseExpression other)
        => true;
}

public abstract record SingleLicense(string Identifier, ImmutableArray<string> Deprecated) : LicenseExpression()
{
    public override string Expression => Identifier;
}

public sealed record PermissiveLicense : SingleLicense
{
    public PermissiveLicense(string identifier, ImmutableArray<string>? deprecated = null)
        : base(identifier, deprecated ?? [])
    {

    }

    public override bool CompatibleWith(LicenseExpression other)
        => true;
}

public sealed record CopyLeftLicense : SingleLicense
{
    public CopyLeftLicense(
        string identifier,
        ImmutableArray<string>? deprecated = null,
        ImmutableArray<string>? compatibilities = null)
        : base(identifier, deprecated ?? [])
    {
        Compatibilities = compatibilities ?? [];
    }

    public ImmutableArray<string> Compatibilities { get; }

    public override bool CompatibleWith(LicenseExpression other)
    {
        if (other.Expression == Expression || Compatibilities.Contains(other.Expression))
        {
            return true;
        }

        return other switch
        {
            // NB: the inversion of the `and` and `or` are intentional.
            AndLicenseExpression e => CompatibleWith(e.Left) || CompatibleWith(e.Right),
            OrLicenseExpression e => CompatibleWith(e.Left) && CompatibleWith(e.Right),
            _ => false, // All other cases.
        };
    }
}

public sealed record AndLicenseExpression(LicenseExpression Left, LicenseExpression Right) : LicenseExpression()
{
    public override string Expression => $"({Left} AND {Right})";

    public override bool CompatibleWith(LicenseExpression other)
        => Left.CompatibleWith(other) && Right.CompatibleWith(other);
}

public sealed record OrLicenseExpression(LicenseExpression Left, LicenseExpression Right) : LicenseExpression()
{
    public override string Expression => $"({Left} OR {Right})";

    public override bool CompatibleWith(LicenseExpression other)
        => Left.CompatibleWith(other) || Right.CompatibleWith(other);
}
