using DotNetProjectFile.Collections;
using Grammr;
using Grammr.Lexers;
using static Grammr.Lexers.Shared;

namespace DotNetProjectFile.Json;

internal static partial class JsonFileParser
{
    private static readonly Lexer whitespace = reg(@"^[ \t\r\n]+", Kind.Whitespace);
    private static readonly Lexer ws = whitespace.optional;

    private static readonly Lexer lbrace = ch('{', Kind.LBrace);
    private static readonly Lexer rbrace = ch('}', Kind.RBrace);
    private static readonly Lexer lbracket = ch('[', Kind.LBracket);
    private static readonly Lexer rbracket = ch(']', Kind.RBracket);
    private static readonly Lexer comma = ch(',', Kind.Comma);
    private static readonly Lexer colon = ch(':', Kind.Colon);

    private static readonly Lexer @null = str("null", Kind.Null);
    private static readonly Lexer @true = str("true", Kind.True);
    private static readonly Lexer @false = str("false", Kind.False);

    private static readonly Lexer number = reg(@"^-?(?:0|[1-9]\d*)(?:\.\d+)?(?:[eE][+-]?\d+)?", Kind.Number);
    private static readonly Lexer stringLexer = reg(@"^""[^""\\\r\n]*(?:\\.[^""\\\r\n]*)*""", Kind.String);

    private static readonly Lexer unparsableLexer = reg(@"^.", Kind.Unparsable);

    public static JsonFile Parse(GrammrTree tree)
    {
        SourceReader reader = new(tree.SourceText);

        _ = reader.Keep(ws);
        var val = Value(ref reader, tree);
        _ = reader.Keep(ws);

        while (!reader.EOS)
        {
            _ = UnparsableToken(ref reader, tree);
        }

        var file = new JsonFile(reader.Stream.Count, tree);
        if (val is not null)
        {
            file.AddChild(val);
        }
        tree.Finalize(reader.Stream);

        return file;
    }

    private static JsonValue? Value(ref SourceReader reader, GrammrTree tree)
    {
        _ = reader.Keep(ws);
        return true switch
        {
            _ when Object(ref reader, tree) is { } obj => obj,
            _ when Array(ref reader, tree) is { } arr => arr,
            _ when String(ref reader, tree) is { } strNode => strNode,
            _ when Number(ref reader, tree) is { } numNode => numNode,
            _ when Boolean(ref reader, tree) is { } boolNode => boolNode,
            _ => Null(ref reader, tree),
        };
    }

    private static JsonObject? Object(ref SourceReader reader, GrammrTree tree)
    {
        var read = reader;
        if (Chain
            && read.Keep(ws)
            && read.Keep(lbrace))
        {
            var properties = new List<GrammrNode>();
            while (Chain
                && !read.EOS)
            {
                _ = read.Keep(ws);
                if (read.Keep(rbrace))
                {
                    break;
                }

                if (Property(ref read, tree) is { } prop)
                {
                    properties.Add(prop);
                    _ = read.Keep(ws);
                    if (!read.Keep(comma))
                    {
                        _ = read.Keep(ws);
                        if (read.Keep(rbrace))
                        {
                            break;
                        }
                        if (UnparsableToken(ref read, tree) is { } unp)
                        {
                            properties.Add(unp);
                        }
                    }
                }
                else
                {
                    _ = read.Keep(ws);
                    if (read.Keep(comma))
                    {
                        continue;
                    }
                    if (read.Keep(rbrace))
                    {
                        break;
                    }
                    if (UnparsableToken(ref read, tree) is { } unp)
                    {
                        properties.Add(unp);
                    }
                }
            }
            var span = SliceSpan.Delta(read.Stream, reader.Stream);
            reader = read;
            var obj = new JsonObject(span, tree);
            obj.AddChildren(properties);
            return obj;
        }
        return null;
    }

    private static JsonProperty? Property(ref SourceReader reader, GrammrTree tree)
    {
        var read = reader;
        if (Chain
            && read.Keep(ws)
            && String(ref read, tree) is { } key
            && read.Keep(ws))
        {
            GrammrNode? val = null;
            if (read.Keep(colon))
            {
                _ = read.Keep(ws);
                val = Value(ref read, tree);
            }

            var span = SliceSpan.Delta(read.Stream, reader.Stream);
            reader = read;
            var prop = new JsonProperty(span, tree);
            prop.AddChildren([key, val]);
            return prop;
        }
        return null;
    }

    private static JsonArray? Array(ref SourceReader reader, GrammrTree tree)
    {
        var read = reader;
        if (Chain
            && read.Keep(ws)
            && read.Keep(lbracket))
        {
            var items = new List<GrammrNode>();
            while (Chain
                && !read.EOS)
            {
                _ = read.Keep(ws);
                if (read.Keep(rbracket))
                {
                    break;
                }

                if (Value(ref read, tree) is { } item)
                {
                    items.Add(item);
                    _ = read.Keep(ws);
                    if (!read.Keep(comma))
                    {
                        _ = read.Keep(ws);
                        if (read.Keep(rbracket))
                        {
                            break;
                        }
                        if (UnparsableToken(ref read, tree) is { } unp)
                        {
                            items.Add(unp);
                        }
                    }
                }
                else
                {
                    _ = read.Keep(ws);
                    if (read.Keep(comma))
                    {
                        continue;
                    }
                    if (read.Keep(rbracket))
                    {
                        break;
                    }
                    if (UnparsableToken(ref read, tree) is { } unp)
                    {
                        items.Add(unp);
                    }
                }
            }
            var span = SliceSpan.Delta(read.Stream, reader.Stream);
            reader = read;
            var arr = new JsonArray(span, tree);
            arr.AddChildren(items);
            return arr;
        }
        return null;
    }

    private static JsonString? String(ref SourceReader reader, GrammrTree tree)
    {
        var read = reader;
        if (Chain
            && read.Keep(ws)
            && read.Keep(stringLexer))
        {
            var span = SliceSpan.Delta(read.Stream, reader.Stream);
            reader = read;
            return new JsonString(span, tree);
        }
        return null;
    }

    private static JsonNumber? Number(ref SourceReader reader, GrammrTree tree)
    {
        var read = reader;
        if (Chain
            && read.Keep(ws)
            && read.Keep(number))
        {
            var span = SliceSpan.Delta(read.Stream, reader.Stream);
            reader = read;
            return new JsonNumber(span, tree);
        }
        return null;
    }

    private static JsonBoolean? Boolean(ref SourceReader reader, GrammrTree tree)
    {
        var read = reader;
        if (Chain
            && read.Keep(ws)
            && (read.Keep(@true) || read.Keep(@false)))
        {
            var span = SliceSpan.Delta(read.Stream, reader.Stream);
            reader = read;
            return new JsonBoolean(span, tree);
        }
        return null;
    }

    private static JsonNull? Null(ref SourceReader reader, GrammrTree tree)
    {
        var read = reader;
        if (Chain
            && read.Keep(ws)
            && read.Keep(@null))
        {
            var span = SliceSpan.Delta(read.Stream, reader.Stream);
            reader = read;
            return new JsonNull(span, tree);
        }
        return null;
    }

    private static JsonUnparsable? UnparsableToken(ref SourceReader reader, GrammrTree tree)
    {
        var read = reader;
        if (read.Keep(unparsableLexer))
        {
            var span = SliceSpan.Delta(read.Stream, reader.Stream);
            reader = read;
            return new JsonUnparsable(span, tree);
        }
        return null;
    }

    internal static class Kind
    {
        public const string Whitespace = nameof(Whitespace);
        public const string LBrace = nameof(LBrace);
        public const string RBrace = nameof(RBrace);
        public const string LBracket = nameof(LBracket);
        public const string RBracket = nameof(RBracket);
        public const string Comma = nameof(Comma);
        public const string Colon = nameof(Colon);
        public const string Null = nameof(Null);
        public const string True = nameof(True);
        public const string False = nameof(False);
        public const string Number = nameof(Number);
        public const string String = nameof(String);
        public const string Unparsable = nameof(Unparsable);
    }
}
