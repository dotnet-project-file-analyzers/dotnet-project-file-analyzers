using DotNetProjectFile.Collections;
using Grammr;
using Grammr.Lexers;
using static Grammr.Lexers.Shared;

namespace DotNetProjectFile.Ini;

internal static partial class IniFileParser
{
    private static readonly Lexer space = ws.optional;
    private static readonly Lexer end = eol | eos;
    private static readonly Lexer comment = (line_comment("#") | line_comment(";")).optional;

    private static readonly Lexer colon = ch(':', Kind.ColonToken);
    private static readonly Lexer equals = ch('=', Kind.EqualToken);
    private static readonly Lexer assign = equals | colon;

    private static readonly Lexer key = reg(@"^[a-zA-Z0-9_\-\.]+", Kind.KeyToken);
    private static readonly Lexer value = reg(@"^[^=:^\s#;][^\s#;]*", Kind.ValueToken);

    private static readonly Lexer header_start = ch('[', Kind.HeaderStart);
    private static readonly Lexer header_end = ch(']', Kind.HeaderEnd);
    private static readonly Lexer header_text = match(IsHeaderText, Kind.HeaderText);

    private static readonly Lexer unparsable = line(Kind.Unparsable).optional;

    private static bool IsHeaderText(char c, int _) => c is not ']' and not '\n' and not '\r';

    public static IniFile Parse(GrammrTree tree)
    {
        var sections = new List<IniSection>();
        var entries = new List<IniEntry>();
        IniHeader? iniHeader = null;

        SourceReader reader = new(tree.SourceText);

        while (!reader.EOS)
        {
            if (Entry(ref reader, tree) is { } e)
            {
                entries.Add(e);
            }
            else if (Header(ref reader, tree) is { } h)
            {
                AddSection(reader, tree);
                iniHeader = h;
            }
            else _ = Choise
                || WhitespaceOrComment(ref reader)
                || UnparsableLine(ref reader);
        }

        AddSection(reader, tree);

        var file = new IniFile(reader.Stream.Count, tree);
        file.AddChildren(sections);
        tree.Finalize(reader.Stream);

        return file;

        void AddSection(SourceReader reader, GrammrTree tree)
        {
            if (iniHeader is not null || entries.Any())
            {
                var offset = sections.LastOrDefault()?.TokenSpan.End ?? 0;
                var span = new SliceSpan(offset, reader.Stream.Count - offset);
                var section = new IniSection(span, tree);
                section.AddChildren([iniHeader, .. entries]);
                sections.Add(section);
                entries.Clear();
            }
        }
    }

    private static IniHeader? Header(ref SourceReader reader, GrammrTree tree)
    {
        var read = reader;

        if (Chain
            && read.Keep(space)
            && read.Keep(header_start)
            && read.Keep(header_text.optional)
            && read.Keep(header_end.optional)
            && read.Keep(space)
            && read.Keep(comment)
            && read.Keep(unparsable)
            && read.Keep(end))
        {
            var span = SliceSpan.Delta(read.Stream, reader.Stream);
            reader = read;
            return new IniHeader(span, tree);
        }
        else return null;
    }

    private static IniEntry? Entry(ref SourceReader reader, GrammrTree tree)
    {
        var read = reader;

        if (Chain
            && Key(ref read, tree) is { } k
            && read.Keep(assign.optional)
            && Value(ref read, tree) is var v
            && read.Keep(space)
            && read.Keep(comment)
            && read.Keep(unparsable)
            && read.Keep(end))
        {
            var span = SliceSpan.Delta(read.Stream, reader.Stream);
            reader = read;
            var entry = new IniEntry(span, tree);
            entry.AddChildren([k, v]);
            return entry;
        }
        else return null;
    }

    private static IniKey? Key(ref SourceReader reader, GrammrTree tree)
    {
        var read = reader;

        if (Chain
            && read.Keep(space)
            && read.Keep(key)
            && read.Keep(space))
        {
            var span = SliceSpan.Delta(read.Stream, reader.Stream);
            reader = read;
            return new(span, tree);
        }
        else return null;
    }

    private static IniValue? Value(ref SourceReader reader, GrammrTree tree)
    {
        var read = reader;

        if (Chain
            && read.Keep(space)
            && read.Keep(value)
            && read.Keep(space))
        {
            var span = SliceSpan.Delta(read.Stream, reader.Stream);
            reader = read;
            return new(span, tree);
        }
        else return null;
    }

    private static bool WhitespaceOrComment(ref SourceReader reader)
    {
        var read = reader;
        if (Chain
            && read.Keep(space)
            && read.Keep(comment)
            && read.Keep(eol))
        {
            reader = read;
            return true;
        }
        else return false;
    }

    private static bool UnparsableLine(ref SourceReader reader)
    {
        reader.Keep(unparsable);
        reader.Keep(eol);
        return true;
    }

    internal static class Kind
    {
        public const string ColonToken = nameof(ColonToken);
        public const string EqualToken = nameof(EqualToken);
        public const string HeaderStart = nameof(HeaderStart);
        public const string HeaderEnd = nameof(HeaderEnd);
        public const string HeaderText = nameof(HeaderText);
        public const string KeyToken = nameof(KeyToken);
        public const string ValueToken = nameof(ValueToken);
        public const string Unparsable = nameof(Unparsable);
    }
}
