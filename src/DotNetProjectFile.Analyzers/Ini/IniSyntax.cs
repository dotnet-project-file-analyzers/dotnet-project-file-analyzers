using Antlr4.Runtime;
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
}
