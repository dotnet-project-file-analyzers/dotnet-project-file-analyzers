// This file contains nodes that are not used yet.

namespace DotNetProjectFile.MsBuild;

/// <summary>Specifies additional folders in which compilers should look for reference assemblies.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class AdditionalLibPaths(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Causes the compiler to make all type information from the specified files available to the project you're compiling. This property is equivalent to the /addModules compiler switch.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class AddModules(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The path to AL.exe. This property overrides the current version of AL.exe to enable use of a different version.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class ALToolPath(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The .ico icon file to pass to the compiler for embedding as a Win32 icon. The property is equivalent to the /win32icon compiler switch.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class ApplicationIcon(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>
/// Specifies the path of the file that is used to generate external User Account Control (UAC) manifest information. Applies only to Visual Studio projects targeting Windows Vista.
///
/// In most cases, the manifest is embedded. However, if you use Registration Free COM or ClickOnce deployment, then the manifest can be an external file that is installed together with your application assemblies. For more information, see the NoWin32Manifest property in this article.
/// </summary>

internal sealed class ApplicationManifest(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies the file used to sign the assembly (.snk or .pfx) and that's passed to the ResolveKeySource task to generate the actual key used to sign the assembly.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class AssemblyOriginatorKeyFile(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A list of locations to search during build-time reference assembly resolution. The order in which paths appear in this list is meaningful because paths listed earlier take precedence over later entries.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class AssemblySearchPaths(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The name of the final output assembly after the project is built.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class AssemblyName(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies the base address of the main output assembly. This property is equivalent to the /baseaddress compiler switch.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class BaseAddress(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The top-level folder where all configuration-specific intermediate output folders are created. The default value is obj\. The following code is an example: <BaseIntermediateOutputPath>c:\xyz\obj\</BaseIntermediateOutputPath></summary>
internal sealed class BaseIntermediateOutputPath(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies the base path for the output file. If it's set, MSBuild uses OutputPath = $(BaseOutputPath)\$(Configuration)\. Example syntax: <BaseOutputPath>c:\xyz\bin\</BaseOutputPath></summary>
internal sealed class BaseOutputPath(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean value that indicates whether project references are built or cleaned in parallel when Multi-Proc MSBuild is used. The default value is true, which means that projects will be built in parallel if the system has multiple cores or processors.</summary>
internal sealed class BuildInParallel(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean value that indicates whether project references are built by MSBuild. Automatically set to false if you're building your project in the Visual Studio integrated development environment (IDE), true if otherwise. -p:BuildProjectReferences=false can be specified on the command line to avoid checking that referenced projects are up to date.</summary>
internal sealed class BuildProjectReferences(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>
/// The name of the file that will be used as the "clean cache." The clean cache is a list of generated files to be deleted during the cleaning operation. The file is put in the intermediate output path by the build process.
/// This property specifies only file names that don't have path information.
/// </summary>
internal sealed class CleanFile(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies the code page to use for all source-code files in the compilation. This property is equivalent to the /codepage compiler switch.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class CodePage(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>An optional response file that can be passed to the compiler tasks.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class CompilerResponseFile(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The configuration that you're building, generally Debug or Release, but configurable at the solution and project levels.</summary>
internal sealed class Configuration(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

internal sealed class CopyLocalLockFileAssemblies(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The path of csc.exe, the C# compiler.</summary>
/// <remarks>C#</remarks>
internal sealed class CscToolPath(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The name of a project file or targets file that is to be imported automatically after the common targets import.</summary>
internal sealed class CustomAfterMicrosoftCommonTargets(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The name of a project file or targets file that is to be imported automatically before the common targets import.</summary>
internal sealed class CustomBeforeMicrosoftCommonTargets(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>
/// A boolean value that indicates whether symbols are generated by the build.
///
/// Setting -p:DebugSymbols=false on the command line disables generation of program database (.pdb) symbol files.
/// </summary>
internal sealed class DebugSymbols(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Defines the level of debug information that you want generated. Valid values are "full," "pdbonly," "portable", "embedded", and "none."</summary>
internal sealed class DebugType(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Defines conditional compiler constants. Symbol/value pairs are separated by semicolons and are specified by using language-dependent syntax:</summary>
/// <summary>C#: symbol1; symbol2</summary>
/// <summary>Visual Basic: symbol1 = value1, symbol2 = value2</summary>
/// <summary>The property is equivalent to the /define compiler switch.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class DefineConstants(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean value that indicates whether you want the DEBUG constant defined.</summary>
internal sealed class DefineDebug(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean value that indicates whether you want the TRACE constant defined.</summary>
internal sealed class DefineTrace(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean value that indicates whether you want to delay-sign the assembly rather than full-sign it.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class DelaySign(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean value that indicates whether the compiler should produce identical assemblies for identical inputs. This parameter corresponds to the /deterministic switch of the compilers.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Deterministic(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies the path to the Directory.Build.props file; if defined, this property overrides the default search algorithm. See Customize your build.</summary>
internal sealed class DirectoryBuildPropsPath(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies the path to the Directory.Build.targets file; if defined, this property overrides the default search algorithm. See Customize your build.</summary>
internal sealed class DirectoryBuildTargetsPath(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean value that applies to Visual Studio only. The Visual Studio build manager uses a process called FastUpToDateCheck to determine whether a project must be rebuilt to be up to date. This process is faster than using MSBuild to determine this. Setting the DisableFastUpToDateCheck property to true lets you bypass the Visual Studio build manager and force it to use MSBuild to determine whether the project is up to date.</summary>
internal sealed class DisableFastUpToDateCheck(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The name of the file that is generated as the XML documentation file. This name includes only the file name and has no path information.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class DocumentationFile(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

internal sealed class EnforceExtendedAnalyzerRules(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies how the compiler task should report internal compiler errors. Valid values are "prompt," "send," or "none." This property is equivalent to the /errorreport compiler switch.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class ErrorReport(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The GenerateDeploymentManifest task adds a deploymentProvider tag to the deployment manifest if the project file includes any of the following elements:</summary>
/// <summary>Using ExcludeDeploymentUrl, however, you can prevent the deploymentProvider tag from being added to the deployment manifest even if any of the above URLs are specified. To do this, add the following property to your project file:</summary>
/// <summary><ExcludeDeploymentUrl>true</ExcludeDeploymentUrl></summary>
/// <summary>Note: ExcludeDeploymentUrl isn't exposed in the Visual Studio IDE and can be set only by manually editing the project file. Setting this property doesn't affect publishing within Visual Studio; that is, the deploymentProvider tag will still be added to the URL specified by PublishUrl.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class ExcludeDeploymentUrl(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies, in bytes, where to align the sections of the output file. Valid values are 512, 1024, 2048, 4096, 8192. This property is equivalent to the /filealignment compiler switch.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class FileAlignment(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean parameter that indicates whether documentation is generated by the build. If true, the build generates documentation information and puts it in an .xml file together with the name of the executable file or library that the build task created.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class GenerateDocumentationFile(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Generate full paths for filenames in output by using the -fullpaths compiler option.</summary>
/// <remarks>C#</remarks>
internal sealed class GenerateFullPaths(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Indicates whether XML serialization assemblies should be generated by SGen.exe, which can be set to on, auto, or off. This property is used for assemblies that target .NET Framework only. To generate XML serialization assemblies for .NET Standard or .NET Core assemblies, reference the Microsoft.XmlSerializer.Generator NuGet package.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class GenerateSerializationAssemblies(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

internal sealed class GlobalAnalyzerConfigFiles(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean value that indicates whether to import a Directory.Build.props file. See Customize your build.</summary>
internal sealed class ImportDirectoryBuildProps(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean value that indicates whether to import a Directory.Build.targets file. See Customize your build.</summary>
internal sealed class ImportDirectoryBuildTargets(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The full intermediate output path as derived from BaseIntermediateOutputPath, if no path is specified. For example, obj\debug\.</summary>
internal sealed class IntermediateOutputPath(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The name of the strong-name key container.</summary>(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }
internal sealed class KeyContainerName(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The name of the strong-name key file.</summary>
internal sealed class KeyOriginatorFile(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

internal sealed class LangVersion(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The name of the assembly that the compiled module is to be incorporated into. The property is equivalent to the /moduleassemblyname compiler switch.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class ModuleAssemblyName(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies the path where project extensions are located. By default, this takes the same value as BaseIntermediateOutputPath.</summary>
internal sealed class MSBuildProjectExtensionsPath(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean value that tells MSBuild to treat all warnings as errors, unless they're suppressed.</summary>
internal sealed class MSBuildTreatWarningsAsErrors(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies a list of warning codes to treat as errors. Separate multiple warnings by semicolons. If you're using the .NET SDK property WarningsAsErrors, MSBuildWarningsAsErrors will default to the value of WarningsAsErrors.</summary>
internal sealed class MSBuildWarningsAsErrors(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies a list of warning codes to suppress by treating them as low-importance messages. Separate multiple warnings by semicolons. Note that some warnings emitted by MSBuild can't be suppressed by using this property; to suppress them, use the command-line switch -warnAsMessage. If you're using the .NET SDK property NoWarn, MSBuildWarningsAsMessages will default to the value of NoWarn.</summary>
internal sealed class MSBuildWarningsAsMessages(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean value that indicates whether you want compiler logo to be turned off. This property is equivalent to the /nologo compiler switch.</summary>
internal sealed class NoLogo(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean value that indicates whether to avoid referencing the standard library (mscorlib.dll). The default value is false.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class NoStdLib(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Suppresses the specified warnings. Only the numeric part of the warning identifier must be specified. Multiple warnings are separated by semicolons. This parameter corresponds to the /nowarn switch of the compilers.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class NoWarn(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>
/// <![CDATA[
/// A boolean value that indicates whether User Account Control (UAC) manifest information will be embedded in the application's executable. Applies only to Visual Studio projects targeting Windows Vista. In projects deployed using ClickOnce and Registration-Free COM, this element is ignored. False (the default value) specifies that User Account Control (UAC) manifest information be embedded in the application's executable. True specifies that UAC manifest information not be embedded.
///
/// This property applies only to Visual Studio projects targeting Windows Vista. In projects deployed using ClickOnce and Registration-Free COM, this property is ignored.
///
/// You should add NoWin32Manifest only if you don't want Visual Studio to embed any manifest information in the application's executable; this process is called virtualization. To use virtualization, set <ApplicationManifest> in conjunction with <NoWin32Manifest> as follows:
/// - For Visual Basic projects, remove the <ApplicationManifest> node. (In Visual Basic projects, <NoWin32Manifest> is ignored when an <ApplicationManifest> node exists.)
/// - For C# projects, set <ApplicationManifest> to False and <NoWin32Manifest> to True. (In C# projects, <ApplicationManifest> overrides <NoWin32Manifest>.)
///
/// This property is equivalent to the /nowin32manifest compiler switch of vbc.exe.
/// ]]>
/// </summary>
/// <remarks>.NET only.</remarks>
internal sealed class NoWin32Manifest(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

internal sealed class Nullable(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean value that when set to true, enables compiler optimizations. This property is equivalent to the /optimize compiler switch.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Optimize(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Indicates the final output location for the project or solution. When building a solution, OutDir can be used to gather multiple project outputs in one location. In addition, OutDir is included in AssemblySearchPaths used for resolving references. For example, bin\Debug.</summary>
internal sealed class OutDir(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies the path to the output directory, relative to the project directory, for example, bin\Debug or bin\Debug\$(Platform) in non-AnyCPU builds.</summary>
internal sealed class OutputPath(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean value that indicates whether you want to enable the build to overwrite read-only files or trigger an error.</summary>
internal sealed class OverwriteReadOnlyFiles(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies how to map physical paths to source path names output by the compiler. This property is equivalent to the /pathmap switch of the compilers.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class PathMap(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The file name of the .pdb file that you're emitting. This property is equivalent to the /pdb switch of the csc.exe compiler.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class PdbFile(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The operating system you're building for. Examples for .NET Framework builds are "Any CPU", "x86", and "x64".</summary>
internal sealed class Platform(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The processor architecture that is used when assembly references are resolved. Valid values are "msil," "x86," "amd64," or "ia64."</summary>
/// <remarks>.NET only.</remarks>
internal sealed class ProcessorArchitecture(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean value that instructs the compiler to emit only a reference assembly rather than compiled code. Can't be used in conjunction with ProduceReferenceAssembly. This property corresponds to the /refonly switch of the vbc.exe and csc.exe compilers.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class ProduceOnlyReferenceAssembly(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean value that when set to true enables production of reference assemblies for the current assembly. Deterministic should be true when using this feature. This property corresponds to the /refout switch of the vbc.exe and csc.exe compilers.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class ProduceReferenceAssembly(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies the default architecture for which the managed DLL is registered. This property is useful because COM uses the Windows registry to store the registrations in architecture-specific hives. For example, on a Windows system, an AnyCPU managed assembly can have its types registered in the 64-bit hive and/or in the 32-bit (WoW) hive, and the build uses this property to determine which architecture-specific registry hive to use. Valid values include "x86," "x64," and "ARM64."</summary>
/// <remarks>Windows only</remarks>
internal sealed class RegisterAssemblyMSBuildArchitecture(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Indicates that your managed application will expose a COM object (a COM callable wrapper). See Build page - Output section. This setting affects only the machine on which the project is building. If you're deploying to other machines, call regasm.exe to register the assembly on the target machine.</summary>
/// <remarks>Windows only</remarks>
internal sealed class RegisterForCOMInterop(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

internal sealed class RepositoryType(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The root namespace to use when you name an embedded resource. This namespace is part of the embedded resource manifest name.</summary>
internal sealed class RootNamespace(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The ID of the AL.exe hashing algorithm to use when satellite assemblies are created.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_AlgorithmId(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The base address to use when culture-specific satellite assemblies are built by using the CreateSatelliteAssemblies target.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_BaseAddress(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The company name to pass into AL.exe during satellite assembly generation.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_CompanyName(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The configuration name to pass into AL.exe during satellite assembly generation.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_Configuration(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The description text to pass into AL.exe during satellite assembly generation.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_Description(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Embeds the specified file in the satellite assembly that has the resource name "Security.Evidence."</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_EvidenceFile(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies a string for the File Version field in the satellite assembly.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_FileVersion(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies a value for the Flags field in the satellite assembly.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_Flags(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Causes the build task to use absolute paths for any files reported in an error message.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_GenerateFullPaths(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Links the specified resource files to a satellite assembly.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_LinkResource(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies the fully-qualified name (that is, class.method) of the method to use as an entry point when a module is converted to an executable file during satellite assembly generation.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_MainEntryPoint(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies a string for the Product field in the satellite assembly.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_ProductName(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies a string for the ProductVersion field in the satellite assembly.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_ProductVersion(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies the file format of the satellite assembly output file as "library," "exe," or "win." The default value is "library."</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_TargetType(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies a string for the Title field in the satellite assembly.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_Title(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies a string for the Trademark field in the satellite assembly.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_Trademark(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies the version information for the satellite assembly.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_Version(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Inserts an .ico icon file in the satellite assembly.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_Win32Icon(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Inserts a Win32 resource (.res file) into the satellite assembly.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Satellite_Win32Resource(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>An optional tool path that indicates where to obtain SGen.exe when the current version of SGen.exe is overridden.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class SGenToolPath(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean value that indicates whether proxy types should be generated by SGen.exe. This applies only when GenerateSerializationAssemblies is set to on.</summary>
/// <summary>The SGen target uses this property to set the UseProxyTypes flag. This property defaults to true, and there's no UI to change this. To generate the serialization assembly for non-webservice types, add this property to the project file and set it to false before importing the Microsoft.Common.Targets or the C#/VB.targets.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class SGenUseProxyTypes(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>When true, generate a warning on invalid platform and configuration combinations, but don't fail the build; when false or undefined (the default), generate an error.</summary>
internal sealed class SkipInvalidConfigurations(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies the class or module that contains the Main method or Sub Main procedure. This property is equivalent to the /main compiler switch.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class StartupObject(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies the minimum version of the subsystem that the generated executable file can use. This property is equivalent to the /subsystemversion compiler switch. For information about the default value of this property, see /subsystemversion (Visual Basic) or /subsystemversion (C# compiler options).</summary>
/// <remarks>.NET only.</remarks>
internal sealed class SubsystemVersion(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

internal sealed class SuppressDependenciesWhenPacking(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The version of the .NET Compact Framework that is required to run the application that you're building. Specifying this lets you reference certain framework assemblies that you may not be able to reference otherwise.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class TargetCompactFramework(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The version of the .NET Framework that is required to run the application that you're building. Specifying this lets you reference certain framework assemblies that you may not be able to reference otherwise.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class TargetFrameworkVersion(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean parameter that, if true, causes all warnings to be treated as errors. This parameter is equivalent to the /nowarn compiler switch.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class TreatWarningsAsErrors(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean property that you can set to true when you want all build outputs in a solution to use the same output directory. If true, referenced projects' output isn't copied to projects that use those dependencies, as is normally the case when this setting is false. Setting this parameter to true doesn't change the actual output directory of any projects; you still need to set the output directory to the desired common output directory for each project that requires it.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class UseCommonOutputDirectory(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean parameter that, if true, causes the build task to use the in-process compiler object, if it's available. This parameter is used only by Visual Studio.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class UseHostCompilerIfAvailable(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>A boolean parameter that, if true, logs compiler output by using UTF-8 encoding. This parameter is equivalent to the /utf8Output compiler switch.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Utf8Output(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies the version of Visual Studio under which this project should be considered to be running. If this property isn't specified, MSBuild sets it to a default value of {VisualStudioMajorVersion}.0; for instance it will be 17.0 for all versions of Visual Studio 2022.</summary>
/// <summary>This property is used in several project types to specify the set of targets that are used for the build. If ToolsVersion is set to 4.0 or higher for a project, VisualStudioVersion is used to specify which sub-toolset to use. For more information, see Toolset (ToolsVersion).</summary>
internal sealed class VisualStudioVersion(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies a list of warnings to treat as errors. This parameter is equivalent to the /warnaserror compiler switch.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class WarningsAsErrors(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies the warning level to pass to the compilers for warnings emitted by the compiler. This setting doesn't affect MSBuild warnings, which don't have level designations. See Warning Level in the C# compiler documentation and /W (Warning level) in the C++ compiler documentation.</summary>
internal sealed class WarningLevel(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>Specifies a list of warnings that aren't treated as errors. This parameter is equivalent to the /warnaserror compiler switch.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class WarningsNotAsErrors(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The name of the manifest file that should be embedded in the final assembly. This parameter is equivalent to the /win32Manifest compiler switch.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Win32Manifest(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }

/// <summary>The file name of the Win32 resource to be embedded in the final assembly. This parameter is equivalent to the /win32resource compiler switch.</summary>
/// <remarks>.NET only.</remarks>
internal sealed class Win32Resource(XElement element, Node? parent, MsBuildProject? project) : Node<bool?>(element, parent, project) { }
