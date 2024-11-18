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
        if (result is { })
        {
            Console.Write($"[OK] ");
        }
        else
        {
            Console.Write("[NO] ");
        }

        Console.Write($"{WS(action()),-20}: '{WS(source.ToString(text))}' => ");

        if (result is { } res)
        {
            Console.WriteLine(WS(source.ToString(res)));
        }
        else
        {
            Console.WriteLine("...");
        }

        static string WS(string str) => str.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t");
    }
}
