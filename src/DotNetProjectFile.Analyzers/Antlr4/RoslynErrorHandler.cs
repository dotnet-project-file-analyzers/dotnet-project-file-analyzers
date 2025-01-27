using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antlr4;

public sealed class RoslynErrorHandler : DefaultErrorStrategy
{
    /// <inheritdoc />
    public override void ReportError(Parser recognizer, RecognitionException e)
    {
        //base.ReportError(recognizer, e);
    }
}
