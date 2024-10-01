using DotNetProjectFile.Collections;

namespace Collections.Substring_specs;

public class Slices
{
    [Test]
    public void substrings()
    {
        Substring str = "Hello, World!";
        var sliced = str.Slice(2, 5);
        sliced.Should().Be((Substring)"llo, ");
    }
}
