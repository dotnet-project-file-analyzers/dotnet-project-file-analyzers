using CodeAnalysis.TestTools.Contexts;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;

namespace Specs.TestTools;

internal sealed class InlineProjectAnalyzerVerifyContextBuilder
{
    private readonly DiagnosticAnalyzer analyzer;
    private readonly ImmutableArray<FileDefinition> files;
    private readonly Lazy<string> hash;
    private readonly Lazy<ProjectAnalyzerVerifyContext> ctx;

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
        this.analyzer = analyzer;
        this.files = files;
        this.hash = new(() =>
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
            var h = GetHash(content);
            return h;
        });
        this.ctx = new(BuildInternal);
    }

    private static string GetHash(string content)
    {
        var input = Encoding.UTF8.GetBytes(content);
        using var md5 = MD5.Create();
        var output = md5.ComputeHash(input);
        var str = Convert.ToHexString(output);
        return str;
    }

    public InlineProjectAnalyzerVerifyContextBuilder WithFile(string name, string? content = null)
    {
        var file = ToFile(name, content);
        return new(analyzer, files.Add(file));
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
        => ctx.Value;

    private ProjectAnalyzerVerifyContext BuildInternal()
    {
        if (files.Length <= 0)
        {
            throw new InvalidOperationException("Requires at least 1 file.");
        }

        var tempDir = Path.Combine(Path.GetTempPath(), "dotnet-project-file-analyzer/tests");
        var dir = Path.Combine(tempDir, hash.Value);
        Directory.CreateDirectory(dir);

        foreach (var file in files)
        {
            var fileName = Path.Combine(dir, file.Name);
            var fileDir = Path.GetDirectoryName(fileName);
            Directory.CreateDirectory(fileDir!);

            if (!File.Exists(fileName) || GetHash(File.ReadAllText(fileName).Trim()) != file.Hash)
            {
                File.WriteAllText(fileName, file.Content);
            }
        }

        var firstFileName = Path.Combine(dir, files[0].Name);
        var fileInfo = new FileInfo(firstFileName);
        
        return ProjectFileAnalyzersDiagnosticAnalyzerExtensions.ForTestProject(analyzer, fileInfo);
    }

    private sealed record FileDefinition
    {
        public required string Name { get; init; }

        public required string Content { get; init; }

        public required string Hash { get; init; }
    }
}

internal static class InlineProjectAnalyzerVerifyContextBuilderExtensions
{
    public static void HasNoIssues(this InlineProjectAnalyzerVerifyContextBuilder builder)
        => builder.Build().HasNoIssues();

    public static void HasIssue(
        this InlineProjectAnalyzerVerifyContextBuilder builder,
        Issue issue)
        => builder.Build().HasIssue(issue);

    public static void HasIssues(
        this InlineProjectAnalyzerVerifyContextBuilder builder,
        params Issue[] issues)
        => builder.Build().HasIssues(issues);
}
