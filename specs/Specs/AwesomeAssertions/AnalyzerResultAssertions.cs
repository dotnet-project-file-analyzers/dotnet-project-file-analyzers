using AwesomeAssertions.Execution;
using Buildalyzer;
using System.Diagnostics;
using System.IO;

namespace AwesomeAssertions;

public sealed class AnalyzerResultAssertions(IAnalyzerResult subject, AssertionChain? chain = null)
{
    public IAnalyzerResult Subject { get; } = subject;

    [DebuggerStepThrough]
    public AndConstraint<AnalyzerResultAssertions> HaveProperties(Dictionary<string, string> expected)
    {
        var properties = Subject.Properties;

        properties.Should().Contain(expected);

        return new(this);
    }

    [DebuggerStepThrough]
    public AndConstraint<AnalyzerResultAssertions> HaveCompilerVisibleProperties(params string[] expected)
    {
        var items = (Subject.Items.TryGetValue("CompilerVisibleProperty", out var actual) ? actual : [])
           .Select(x => x.ItemSpec)
           .Order()
           .ToArray();

        items.Should().Contain(expected);

        return new(this);
    }

    [DebuggerStepThrough]
    public AndConstraint<AnalyzerResultAssertions> HaveContent(params Specs.TestTools.ProjectItem[] expected)
        => HaveItems("Content", expected);

    [DebuggerStepThrough]
    public AndConstraint<AnalyzerResultAssertions> HaveAdditionalFiles(params Specs.TestTools.ProjectItem[] expected)
        => HaveItems("AdditionalFiles", expected);

    [DebuggerStepThrough]
    public AndConstraint<AnalyzerResultAssertions> HaveItems(string name, params Specs.TestTools.ProjectItem[] expected)
    {
        TextWriter? writer = null;
        AttachConsole(ref writer);

        var acts = Subject.Items.OfType(name).ToDictionary(i => i.ItemSpec, i => i);
        var exps = expected.ToDictionary(i => i.ItemSpec, i => i);

        var valid = true;
        var sb = new StringBuilder();

        foreach (var (key, a) in acts)
        {
            if (!exps.ContainsKey(key))
            {
                valid &= Compare(a, null, sb);
            }
        }

        foreach (var (key, e) in exps)
        {
            acts.TryGetValue(key, out var a);
            valid &= Compare(a, e, sb);
        }

        if (!valid) throw new AssertionException(sb.ToString());

        writer?.Write(sb);

        return new(this);
    }

    private static bool Compare(IProjectItem? act, Specs.TestTools.ProjectItem? exp, StringBuilder sb)
    {
        var valid = true;

        if (act is null)
        {
            sb.AppendLine($"[-] {exp!.ItemSpec}");
            valid = false;
        }
        else if (exp is null)
        {
            sb.AppendLine($"[+] {act.ItemSpec}");
            foreach (var (name, a) in act.Metadata.OrderBy(kvp => kvp.Key))
            {
                sb.AppendLine($" * [+] {name}: {a}");
            }
            valid = false;
        }
        else
        {
            sb.Append($"[ ] {exp.ItemSpec}");
            sb.AppendLine();
            foreach (var (name, e) in exp.Metadata.OrderBy(kvp => kvp.Key))
            {
                if (!act.Metadata.TryGetValue(name, out var a))
                {
                    sb.AppendLine($" * [-] {name}: {e}");
                    valid = false;
                }
                else if (a != e)
                {
                    sb.AppendLine($" * [x] {name}: {e} != {a}");
                    valid = false;
                }
                else
                {
                    sb.AppendLine($" * [ ] {name}: {e}");
                }
            }
            foreach (var (name, a) in act.Metadata.OrderBy(kvp => kvp.Key))
            {
                if (!exp.Metadata.ContainsKey(name))
                {
                    sb.AppendLine($" * [+] {name}: {a}");
                    valid = false;
                }
            }
        }

        sb.AppendLine();

        return valid;
    }

    /// <summary>Attaches the console in DEBUG mode.</summary>
    [Conditional("DEBUG")]
    private static void AttachConsole(ref TextWriter? writer)
        => writer ??= Console.Out;
}
