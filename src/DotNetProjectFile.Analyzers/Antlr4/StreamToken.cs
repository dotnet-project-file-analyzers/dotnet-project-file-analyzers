using System;
using System.Collections.Generic;
using System.Text;

namespace Antlr4;

[DebuggerDisplay("{Text}, Kind = {Kind}")]
public readonly struct StreamToken(string text, string kind)
{
    public string Text { get; } = text;

    public string Kind { get; } = kind;
}
