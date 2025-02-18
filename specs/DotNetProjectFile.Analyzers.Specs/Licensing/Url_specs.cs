using DotNetProjectFile.Licensing;

namespace Licensing.Url_specs;

public class From_url
{
    [TestCase(null, null)]
    [TestCase("", null)]
    [TestCase("www.github.com/", null)]
    [TestCase("https://opensource.org/licenses/Apache-2.0", "Apache-2.0")]
    [TestCase("https://opensource.org/licenses/Apache-2.0/", "Apache-2.0")]
    [TestCase("http://opensource.org/licenses/Apache-2.0", "Apache-2.0")]
    [TestCase("http://opensource.org/licenses/Apache-2.0/", "Apache-2.0")]
    [TestCase("opensource.org/licenses/Apache-2.0", "Apache-2.0")]
    [TestCase("opensource.org/licenses/Apache-2.0/", "Apache-2.0")]
    [TestCase("https://www.opensource.org/licenses/Apache-2.0", "Apache-2.0")]
    [TestCase("https://www.opensource.org/licenses/Apache-2.0/", "Apache-2.0")]
    [TestCase("http://www.opensource.org/licenses/Apache-2.0", "Apache-2.0")]
    [TestCase("http://www.opensource.org/licenses/Apache-2.0/", "Apache-2.0")]
    [TestCase("www.opensource.org/licenses/Apache-2.0", "Apache-2.0")]
    [TestCase("www.opensource.org/licenses/Apache-2.0/", "Apache-2.0")]
    [TestCase("https://licenses.nuget.org/MIT", "MIT")]
    [TestCase("https://licenses.nuget.org/MIT/", "MIT")]
    [TestCase("http://licenses.nuget.org/MIT", "MIT")]
    [TestCase("http://licenses.nuget.org/MIT/", "MIT")]
    [TestCase("licenses.nuget.org/MIT", "MIT")]
    [TestCase("licenses.nuget.org/MIT/", "MIT")]
    [TestCase("https://www.licenses.nuget.org/MIT", "MIT")]
    [TestCase("https://www.licenses.nuget.org/MIT/", "MIT")]
    [TestCase("http://www.licenses.nuget.org/MIT", "MIT")]
    [TestCase("http://www.licenses.nuget.org/MIT/", "MIT")]
    [TestCase("www.licenses.nuget.org/MIT", "MIT")]
    [TestCase("www.licenses.nuget.org/MIT/", "MIT")]
    [TestCase("https://spdx.org/licenses/BSD-3-Clause.html", "BSD-3-Clause")]
    [TestCase("http://spdx.org/licenses/BSD-3-Clause.html/", "BSD-3-Clause")]
    [TestCase("www.spdx.org/licenses/BSD-3-Clause.html", "BSD-3-Clause")]

    [TestCase("https://ianhammondcooper.mit-license.org/", "MIT")]
    [TestCase("https://microsoft.mit-license.org/", "MIT")]
    [TestCase("https://www.apache.org/licenses/LICENSE-2.0", "Apache-2.0")]
    [TestCase("https://www.gnu.org/licenses/lgpl.html", "LGPL-3.0-only")]
    [TestCase("https://www.gnu.org/licenses/agpl.html", "AGPL-3.0-only")]
    [TestCase("https://www.gnu.org/licenses/gpl.html", "GPL-3.0-only")]
    [TestCase("https://www.gnu.org/licenses/fdl.html", "GFDL-1.3-only")]
    [TestCase("https://www.opensource.org/licenses/bsd-license.php", "BSD-2-Clause")]
    [TestCase("https://www.opensource.org/licenses/mit-license.php", "MIT")]
    public void Results_in(string? url, string? expected)
    {
        expected ??= Licenses.Unknown.Expression;

        Licenses.FromUrl(url).Expression.Should().Be(expected);
    }
}
