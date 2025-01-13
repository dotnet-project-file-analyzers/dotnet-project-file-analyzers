# Grammr
Grammr contains a set of lexers and parsers. It aims to define a
[formal grammar](https://en.wikipedia.org/wiki/Formal_grammar)
in a way that the code itself reads as a formal grammar itself.

## Lexers
There are several ways to construct lexers that match single tokens:

``` C#
public sealed class CustomGrammar : Grammar
{
    // End of line.
    Token EndOfLine = eol();

    // Single char.
    Token C = ch('C');

    // Collection of chars.
    Token ABC = str("ABC");

    // Based on Predicate<char>.
    Token WhiteSpace = match(char.IsWhiteSpace);

    // Based on a regular expression.
    Token Digits = regex("[0-9]+");
}
```

## Parsers
Parsers can combine other parsers (lexers are a special kind of parser).

``` C#
public sealed class CustomGrammar : Grammar
{
    // Sequence
    Parser Sequence = ch('A') & ch('B');

    // Switch
    Parser Switch = ch('A') | ch('B');

    // Zero or one.
    Parser Options = ch('A')[..1]; // ch('A').Option;

    // Zero or more.
    Parser Options = ch('A')[0..]; // ch('A').Star;

    // One or more.
    Parser Options = ch('A')[1..]; // ch('A').Star;

    // Repeat
    Parser Options = ch('A')[a..b]; // ch('A').Repeat(a, b);

    // Use a custom node type to contain child nodes/tokens.
    Typed Typed = node<CustomNode>(Options);
}
```
