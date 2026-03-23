using DotNetProjectFile.MsBuild.Conversion;
using System.Runtime.CompilerServices;

namespace DotNetProjectFile.MsBuild;

/// <summary>Accessor to the (compiler visible) MSBuild properties.</summary>
public sealed class MsBuildProps(AnalyzerOptions options)
{
    /// <summary>Gets the MSBuild project file that currently is being build.</summary>
    public string? MSBuildProjectFile => Prop();

    /// <summary>Indicates if the project is a test project.</summary>
    public bool? IsTestProject => Converts.String<bool>(Prop());

    /// <summary>Gets the .NET CoreSdk version.</summary>
    public SdkVersion NETCoreSdkVersion => SdkVersion.Parse(Prop() ?? string.Empty);

    /// <summary>The Package license expression (such as MIT, etc...).</summary>
    public string? PackageLicenseExpression => Prop();

    /// <summary>Gets the MS Build property.</summary>
    [Pure]
    private string? Prop([CallerMemberName] string? propertyName = null)
        => Options.AnalyzerConfigOptionsProvider.GlobalOptions
            .TryGetValue($"build_property.{propertyName}", out var value)
            ? value
            : null;

    private readonly AnalyzerOptions Options = options;
}
