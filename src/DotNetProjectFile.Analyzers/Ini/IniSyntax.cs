using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetProjectFile.Ini;

[DebuggerDisplay("Name = {Name}, Text = {Text}")]
public class IniSyntax(ParserRuleContext context)
{
	protected ParserRuleContext Context { get; } = context;

	public string Text => Context.GetText();

	private string Name => GetType().Name;

    public IReadOnlyList<IParseTree> Tokens => Context.children.ToArray();
}
