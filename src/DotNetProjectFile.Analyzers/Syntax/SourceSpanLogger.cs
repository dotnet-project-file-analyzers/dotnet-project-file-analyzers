using Microsoft.CodeAnalysis.Text;
using System.Net.NetworkInformation;

namespace DotNetProjectFile.Syntax;

#pragma warning disable RS1035 // Do not use APIs banned for analyzers
internal static class SourceSpanLogger
{
    [Conditional("DEBUG")]
    internal static void Log(SourceText source, TextSpan text, TextSpan? result, Predicate<char> predicate)
    {
        var method = predicate.Method.Name;

        Log(source, text, result, () => $"{method}()");
    }

    [Conditional("DEBUG")]
    internal static void Log(SourceText source, TextSpan text, TextSpan? result, Func<string> action)
    {
        Console.Write($"{WS(action()),-20}: '{WS(source.ToString(text))}' => ");

        if (result is { } res)
        {
            Console.WriteLine($"OK: '{WS(source.ToString(res))}'");
        }
        else
        {
            Console.WriteLine("NO");
        }

        static string WS(string str) => str.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t");
    }
}
