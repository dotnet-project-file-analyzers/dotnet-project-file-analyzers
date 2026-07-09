using System.IO;

namespace Specs.TestTools;

public static class Streams
{
    public static Stream FromText(string text)
        => new MemoryStream(Encoding.UTF8.GetBytes(text));
}
