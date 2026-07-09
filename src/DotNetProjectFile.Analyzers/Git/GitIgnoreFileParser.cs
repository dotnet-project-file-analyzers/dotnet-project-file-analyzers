using DotNetProjectFile.Collections;
using Grammr;
using Grammr.Lexers;
using static Grammr.Lexers.Shared;

namespace DotNetProjectFile.Git;

internal static class GitIgnoreFileParser
{
    private static readonly Lexer comment = line_comment("#", Kind.CommentToken).optional;
    private static readonly Lexer space = ws.optional;
    private static readonly Lexer end = eol | eos;
    private static readonly Lexer unparsable = line(Kind.UnparsableToken);

    public static GitIgnoreFile Parse(GrammrTree tree)
    {
        var entries = new List<GitIgnoreEntry>();
        var reader = new SourceReader(tree.SourceText);

        while (!reader.EOS)
        {
            if (Trivia(ref reader))
            {
                // continue.
            }
            else if (Entry(ref reader, tree) is { } entry)
            {
                entries.Add(entry);
            }
            else
            {
                // unparsable adds the line.
                var count = reader.Stream.Count;
                reader.Keep(unparsable);
                reader.Keep(end);
                entries.Add(new(new(count, reader.Stream.Count - count), tree));
            }
        }
        var file = new GitIgnoreFile(reader.Stream.Count, tree);
        file.AddChildren(entries);
        tree.Finalize(reader.Stream);
        return file;
    }

    private static GitIgnoreEntry? Entry(ref SourceReader reader, GrammrTree tree)
    {
        var read = reader;
        if (Chain
            && read.Keep(space)
            && Glob(ref read) is { } parsed
            && read.Keep(space)
            && read.Keep(comment)
            && read.Keep(end))
        {
            var span = new SliceSpan(reader.Stream.Count, read.Stream.Count - reader.Stream.Count);
            reader = read;

            return new(span, tree) { Glob = parsed };
        }
        else return null;
    }

    private static bool Trivia(ref SourceReader reader)
    {
        var read = reader;
        if (Chain
            && read.Keep(space)
            && read.Keep(comment)
            && read.Keep(end))
        {
            reader = read;
            return true;
        }
        else return false;
    }

    private static Glob? Glob(ref SourceReader reader)
    {
        var read = reader;
        if (GlobParser.Parse(ref read) is { } parsed)
        {
            reader = read.Add(new(reader.Start, read.Start - reader.Start, read.Source, Kind.GlobToken));
            return new Glob(parsed);
        }
        else return null;
    }

    private static class Kind
    {
        public const string CommentToken = nameof(CommentToken);
        public const string GlobToken = nameof(GlobToken);
        public const string UnparsableToken = nameof(UnparsableToken);
    }
}
