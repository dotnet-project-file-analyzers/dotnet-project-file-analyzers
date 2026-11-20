using DotNetProjectFile.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Threading;
using VerifyTests;

namespace CodeAnalysis.ProjectFiles_specs;

public class MsBuildProject
{
    [Test]
    public async Task Reflects_edited_buffer_when_file_on_disk_is_unchanged()
    {
        const string saved = "<Project><PropertyGroup><A>1</A></PropertyGroup></Project>";
        const string edited = "<Project><PropertyGroup><A>2</A></PropertyGroup></Project>";

        using var file = await TempFile.CreateText(saved, ".csproj");
        var files = new ProjectFiles();

        files.MsBuildProject(new BufferText(file.Path, saved))!.Text.ToString().Should().Be(saved);

        // Like an IDE: the buffer changes, the file on disk does not (yet).
        files.MsBuildProject(new BufferText(file.Path, edited))!.Text.ToString().Should().Be(edited);
    }
}

/// <summary>Mimics an open editor buffer: text that might differ from disk.</summary>
file sealed class BufferText(string path, string text) : AdditionalText
{
    private readonly SourceText Text = SourceText.From(text);

    public override string Path { get; } = path;

    public override SourceText GetText(CancellationToken cancellationToken = default) => Text;
}
