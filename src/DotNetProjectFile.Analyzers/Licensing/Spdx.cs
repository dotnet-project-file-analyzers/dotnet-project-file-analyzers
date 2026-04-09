using System.Collections.Frozen;

namespace DotNetProjectFile.Licensing;

public static partial class Spdx
{
    public static readonly ImmutableArray<SpdxLicenseInfo> Licenses
        = ReadFromResources();

    private static readonly FrozenDictionary<string, SpdxLicenseInfo> Lookup
        = Licenses.ToFrozenDictionary(x => x.Id, x => x);

    public static SpdxLicenseInfo? TryGetLicense(string? id) => id switch
    {
        not { Length: > 0 } => null,
        _ when Lookup.TryGetValue(id, out var license) => license,
        _ => null,
    };

    private static ImmutableArray<SpdxLicenseInfo> ReadFromResources()
    {
        var asm = typeof(Spdx).Assembly;
        using var resource = asm.GetManifestResourceStream("DotNetProjectFile.Licensing.Generated.spdx_info.bin");
        return [.. SpdxLicenseInfo.ReadAllFromCompressed(resource)];
    }
}
