using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DotNetProjectFile.Licensing;

public static partial class Spdx
{
    public static readonly ImmutableArray<SpdxLicenseInfo> Licenses
        = typeof(Spdx)
        .GetRuntimeFields()
        .Where(f => f.FieldType == typeof(SpdxLicenseInfo))
        .Select(f => f.GetValue(null))
        .OfType<SpdxLicenseInfo>()
        .ToImmutableArray();

    private static readonly FrozenDictionary<string, SpdxLicenseInfo> Lookup
        = Licenses.ToFrozenDictionary(x => x.Id, x => x);

    public static SpdxLicenseInfo? TryGetLicense(string? id)
    {
        if (id is not { Length: > 0 })
        {
            return null;
        }

        return Lookup.TryGetValue(id, out var license)
            ? license
            : null;
    }
}
