using DotNetProjectFile.NuGet;
using DotNetProjectFile.NuGet.Packaging;

namespace Licensing.File_specs;

public class Can_be_determined
{
    [Test]
    public void Foo()
    {
        var all = PackageCache.GetDirectory().Files("/**/*.nuspec")
            .Select(static x =>
            {
                using var stream = x.TryOpenRead();
                try
                {
                    return NuSpecFile.Load(stream);
                }
                catch
                {
                    return null;
                }
            })
            .OfType<NuSpecFile>()
            .Select(nuspec => PackageCache.GetPackage(nuspec.Metadata.Id, nuspec.Metadata.Version))
            .OfType<CachedPackage>()
            .ToArray();

        var missing = all
            .Where(x => x.License.IsUnknown)
            .ToArray();

        var missingWithLicense
            = missing
            .Where(x => x.LicenseFile is { Length: > 0 })
            .ToArray();
    }
}
