using DotNetProjectFile.NuGet;
using System.Collections.Immutable;

namespace NuGet_License_specs;

public class Is
{
    private static readonly ImmutableArray<LicenseExpression> All = Licenses.All;
    private static readonly ImmutableArray<PermissiveLicense> Permissive = All.OfType<PermissiveLicense>().ToImmutableArray();
    private static readonly ImmutableArray<CopyLeftLicense> CopyLeft = All.OfType<CopyLeftLicense>().ToImmutableArray();

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
        public void Permissive_with_copy_left(LicenseExpression permissive)
        {
            foreach (var other in CopyLeft)
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
    public void Compatiblity(string dependency, string target, bool expectedCompatibility)
    {
        var dep = Licenses.FromExpression(dependency);
        var tar = Licenses.FromExpression(target);

        dep.CompatibleWith(tar).Should().Be(expectedCompatibility);
    }
}
