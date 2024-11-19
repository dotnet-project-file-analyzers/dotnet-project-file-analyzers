namespace DotNetProjectFile;

public static partial class Rule
{
    public static class Ini
    {
        public static DiagnosticDescriptor Invalid => New(
           id: 4000,
           title: "Invalid INI file",
           message: "File could not be parsed",
           description: "A INI header should have the format [<Header>].",
           tags: ["INI", "syntax error"],
           category: Category.SyntaxError);

        public static DiagnosticDescriptor InvalidHeader => New(
           id: 4001,
           title: "Invalid INI Header",
           message: "{0}",
           description: "A INI header should have the format [<Header>].",
           tags: ["INI", "syntax error"],
           category: Category.SyntaxError);

        public static DiagnosticDescriptor InvalidKeyValuePair => New(
           id: 4002,
           title: "Invalid INI key-value pair",
           message: "{0}",
           description: "A INI key-value pair should have the format <Key> ( : | = ) <Value>.",
           tags: ["INI", "syntax error"],
           category: Category.SyntaxError);
    }
}
