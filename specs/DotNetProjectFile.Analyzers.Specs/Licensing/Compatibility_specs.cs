using DotNetProjectFile.Licensing;
using DotNetProjectFile.NuGet;
using DotNetProjectFile.NuGet.Packaging;
using System.Collections.Immutable;

namespace Licensing.Compatibility_specs;

public class Is
{
    private static readonly ImmutableArray<LicenseExpression> All = Licenses.All;
    private static readonly ImmutableArray<PermissiveLicense> Permissive = All.OfType<PermissiveLicense>().ToImmutableArray();
    private static readonly ImmutableArray<CopyLeftLicense> CopyLeft = All.OfType<CopyLeftLicense>().ToImmutableArray();
    private static readonly ImmutableArray<PropietaryLicense> Propietary = All.OfType<PropietaryLicense>().ToImmutableArray();

    public sealed class Compatible
    {
        [TestCaseSource(typeof(Is), nameof(All))]
        public void Any_with_self(LicenseExpression license)
        {
            license.CompatibleWith(license).Should().BeTrue();
        }

        [TestCaseSource(typeof(Is), nameof(All))]
        public void Any_with_self_reparsed(LicenseExpression license)
        {
            license.CompatibleWith(license.Expression).Should().BeTrue();
        }

        [TestCaseSource(typeof(Is), nameof(Permissive))]
        public void Permissive_with_all(LicenseExpression permissive)
        {
            foreach (var other in All)
            {
                permissive.CompatibleWith(other).Should().BeTrue();
            }
        }
    }

    public sealed class Not_compatible
    {
        [TestCaseSource(typeof(Is), nameof(CopyLeft))]
        public void Copy_left_with_permissive(LicenseExpression copyLeft)
        {
            foreach (var other in Permissive)
            {
                copyLeft.CompatibleWith(other).Should().BeFalse();
            }
        }

        [TestCaseSource(typeof(Is), nameof(CopyLeft))]
        public void Copy_left_with_propietary(LicenseExpression copyLeft)
        {
            foreach (var other in Propietary)
            {
                copyLeft.CompatibleWith(other).Should().BeFalse();
            }
        }

        [TestCaseSource(typeof(Is), nameof(CopyLeft))]
        public void Copy_left_with_unknown(LicenseExpression copyLeft)
        {
            copyLeft.CompatibleWith(Licenses.Unknown).Should().BeFalse();
        }
    }

    [TestCase("GPL-3.0", "GPL-3.0-only", true)]
    [TestCase("GPL-3.0+", "GPL-3.0-only", true)]
    [TestCase("GPL-3.0-only", "GPL-3.0", true)]
    [TestCase("GPL-3.0-only", "GPL-3.0+", false)]
    [TestCase("GPL-1.0+", "GPL-3.0+", true)]
    [TestCase("GPL-2.0+", "GPL-3.0+", true)]
    [TestCase("GPL-3.0+", "GPL-3.0+", true)]
    [TestCase("GPL-3.0+", "GPL-1.0+", false)]
    [TestCase("GPL-3.0", "AGPL-3.0", true)] // GPL3 clause 13
    [TestCase("AGPL-3.0", "GPL-3.0", false)]
    [TestCase("NET_Library_EULA", "MIT", true)]
    public void Compatiblity(string dependency, string target, bool expectedCompatibility)
    {
        var dep = Licenses.FromExpression(dependency);
        var tar = Licenses.FromExpression(target);

        dep.CompatibleWith(tar).Should().Be(expectedCompatibility);
    }

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
            .Select(x => PackageCache.GetPackage(x.Metadata!.Id, x.Metadata!.Version))
            .OfType<CachedPackage>()
            .ToArray();

        var unknown = all.Where(x => x.License.IsUnknown).ToArray();
    }
}
