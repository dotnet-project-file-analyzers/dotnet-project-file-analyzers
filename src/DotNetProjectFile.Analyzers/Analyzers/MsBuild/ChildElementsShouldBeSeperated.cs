using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.ChildElementsShouldBeSeperated"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ChildElementsShouldBeSeperated()
    : MsBuildProjectFileAnalyzer(Rule.ChildElementsShouldBeSeperated)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.AllExceptDirectoryPackages;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var project = context.File.Project;

        List<Block> blocks =
        [
            new Block(0, context.File.Locations.Start, BlockType.Root),

            new Block(context.File.Locations.End, context.File.Locations.End, BlockType.Root),

            .. project.Element
                .DescendantNodes()
                .OfType<XComment>()
                .Select(x => new XmlComment(x, context.File))
                .Select(CommentBlock),

            .. context.File.Children.Select(NodeBlock)
        ];

        blocks.Sort();

        for (var i = 0; i < blocks.Count - 1; i++)
        {
            var (prev, curr) = (blocks[i], blocks[i + 1]);

            var space = Space(prev.Type, curr.Type);

            if (space != curr.Start - prev.End)
            {
                var line = new LinePositionSpan(new(prev.End, 0), new(prev.End + 1, 0));
                var location = Location.Create(project.Path.ToString(), project.Text.TextSpan(line), line);
                context.ReportDiagnostic(Descriptor, location, "x", "y");
            }
        }
    }

    private static int Space(BlockType prev, BlockType curr) => (prev, curr) switch
    {
        (BlockType.Import, BlockType.Import) => 1,
        (BlockType.Comment, BlockType.Comment) => 2,
        (BlockType.Comment, _) => 1,
        _ => 2,
    };

    private static Block CommentBlock(XmlComment x) => new(x.Locations.Start, x.Locations.End, BlockType.Comment);

    private static Block NodeBlock(Node n) => new(n.Locations.Start, n.Locations.End, n is Import ? BlockType.Import : BlockType.Block);

    private enum BlockType
    {
        Root,
        Comment,
        Block,
        Import,
    }

    private readonly record struct Block(int Start, int End, BlockType Type) : IComparable<Block>
    {
        public int CompareTo(Block other) => Start.CompareTo(other.Start);
    }
}

file static class Extensions
{
    extension(XmlLocations loc)
    {
        public int Start => loc.Positions.StartElement.Start.Line;
        public int End => loc.Positions.EndElement.End.Line;
    }
}
