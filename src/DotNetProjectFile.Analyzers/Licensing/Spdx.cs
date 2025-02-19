using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DotNetProjectFile.Licensing;

public static partial class Spdx
{
    public static readonly ImmutableArray<SpdxLicenseInfo> Licenses
        = ReadFromResources();

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

    private static ImmutableArray<SpdxLicenseInfo> ReadFromResources()
    {
        var asm = typeof(Spdx).Assembly;
        using var resource = asm.GetManifestResourceStream("DotNetProjectFile.Licensing.Generated.spdx_info.bin");
        return SpdxLicenseInfo.ReadAllFromCompressed(resource).ToImmutableArray();
    }
}
