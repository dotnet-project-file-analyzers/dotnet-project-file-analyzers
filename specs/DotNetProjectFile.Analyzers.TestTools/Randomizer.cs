using System;

namespace DotNetProjectFile.Analyzers.TestTools;

public static class Randomizer
{
    public static string NextWord(this Random rnd, int maxLength)
    {
        var length = rnd.Next(3, maxLength);
        var chars = new char[length];

        for( int i = 0; i < length; i++)
        {
            chars[i] = (char)('A' + rnd.Next(26));
        }

        return new(chars);
    }
}
