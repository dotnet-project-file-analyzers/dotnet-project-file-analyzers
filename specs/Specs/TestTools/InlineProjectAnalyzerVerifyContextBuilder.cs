using CodeAnalysis.TestTools.Contexts;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace Specs.TestTools;

internal sealed class InlineProjectAnalyzerVerifyContextBuilder
{
    private static readonly Lock Locker = new();
    private readonly DiagnosticAnalyzer Analyzer;
    private readonly ImmutableArray<FileDefinition> Files;
    private readonly Lazy<string> Hash;
    private readonly Lazy<ProjectAnalyzerVerifyContext> Ctx;

    public InlineProjectAnalyzerVerifyContextBuilder(
        DiagnosticAnalyzer analyzer,
        string file,
        string? content)
        : this(analyzer, [ToFile(file, content)])
    {
    }

    private InlineProjectAnalyzerVerifyContextBuilder(
        DiagnosticAnalyzer analyzer,
        ImmutableArray<FileDefinition> files)
    {
        Analyzer = analyzer;
        Files = files;
        Hash = new(() => GetHash(files));
        Ctx = new(BuildInternal);
    }

    [AssertionMethod]
    public void HasNoIssues() => Build().HasNoIssues();

    [AssertionMethod]
    public void HasIssue(Issue issue) => Build().HasIssue(issue);

    [AssertionMethod]
    public void HasIssues(params Issue[] issues) => Build().HasIssues(issues);

    private static string GetHash(ImmutableArray<FileDefinition> files)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < files.Length; i++)
        {
            var file = files[i];

            sb.AppendLine(i.ToString(CultureInfo.InvariantCulture));
            sb.AppendLine(file.Name);
            sb.AppendLine(file.Content);
            sb.AppendLine();
        }
        var content = sb.ToString();
        return GetHash(content);
    }

    private static string GetHash(string content)
    {
        var input = Encoding.UTF8.GetBytes(content);
        var output = SHA1.HashData(input);
        var str = Convert.ToHexString(output);
        return str;
    }

    public InlineProjectAnalyzerVerifyContextBuilder WithFile(string name, string? content = null)
    {
        var file = ToFile(name, content);
        return new(Analyzer, Files.Add(file));
    }

    private static FileDefinition ToFile(string name, string? content)
    {
        var trimmed = content?.Trim() ?? string.Empty;

        var file = new FileDefinition
        {
            Name = name,
            Content = trimmed,
            Hash = GetHash(trimmed),
        };
        return file;
    }

    public ProjectAnalyzerVerifyContext Build()
        => Ctx.Value;

    private ProjectAnalyzerVerifyContext BuildInternal()
    {
        if (Files.Length <= 0)
        {
            throw new InvalidOperationException("Requires at least 1 file.");
        }

        lock (Locker)
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "dotnet-project-file-analyzer/tests");
            var dir = Path.Combine(tempDir, Hash.Value);
            Directory.CreateDirectory(dir);

            foreach (var file in Files)
            {
                var fileName = Path.Combine(dir, file.Name);
                var fileDir = Path.GetDirectoryName(fileName);
                Directory.CreateDirectory(fileDir!);

                if (!File.Exists(fileName) || GetHash(File.ReadAllText(fileName).Trim()) != file.Hash)
                {
                    File.WriteAllText(fileName, file.Content);
                }
            }

            var firstFileName = Path.Combine(dir, Files[0].Name);
            var fileInfo = new FileInfo(firstFileName);

            return ProjectFileAnalyzersDiagnosticAnalyzerExtensions.ForTestProject(Analyzer, fileInfo);
        }
    }


    private sealed record FileDefinition
    {
        public required string Name { get; init; }

        public required string Content { get; init; }

        public required string Hash { get; init; }
    }
}
