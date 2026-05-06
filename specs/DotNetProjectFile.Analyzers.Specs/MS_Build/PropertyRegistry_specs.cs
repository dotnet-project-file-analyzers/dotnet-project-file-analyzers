using DotNetProjectFile.IO;
using DotNetProjectFile.MsBuild;

namespace MS_Build_PropertyRegistry_specs;

internal static class Fixture
{
    public static readonly IOFile ContainingFile = IOFile.Parse(@"C:\repo\src\Lib\Lib.csproj");
    public static readonly IOFile EntryFile = IOFile.Parse(@"C:\repo\src\App\App.csproj");
    public static readonly PropertyRegistry Registry = new(ContainingFile, EntryFile);

    public static readonly string ThisDir = $@"C:\repo\src\Lib{IOPath.Separator}";
    public static readonly string ProjectDir = @"C:\repo\src\App";
}

public class Lookup_reserved_per_file_properties
{
    [Test]
    public void MSBuildThisFile_returns_filename()
        => Fixture.Registry.Lookup("MSBuildThisFile").Should().Be("Lib.csproj");

    [Test]
    public void MSBuildThisFileDirectory_returns_directory_with_trailing_separator()
        => Fixture.Registry.Lookup("MSBuildThisFileDirectory").Should().Be(Fixture.ThisDir);

    [Test]
    public void MSBuildThisFileExtension_returns_extension_with_dot()
        => Fixture.Registry.Lookup("MSBuildThisFileExtension").Should().Be(".csproj");

    [Test]
    public void MSBuildThisFileFullPath_returns_full_path()
        => Fixture.Registry.Lookup("MSBuildThisFileFullPath").Should().Be(@"C:\repo\src\Lib\Lib.csproj");

    [Test]
    public void MSBuildThisFileName_returns_name_without_extension()
        => Fixture.Registry.Lookup("MSBuildThisFileName").Should().Be("Lib");

#if Is_Windows
    [Test]
    public void MSBuildThisFileDirectoryNoRoot_strips_drive_root_and_keeps_trailing_separator()
        => Fixture.Registry.Lookup("MSBuildThisFileDirectoryNoRoot")
            .Should().Be($@"repo\src\Lib{IOPath.Separator}");
#endif
}

public class Lookup_reserved_per_project_properties
{
    [Test]
    public void MSBuildProjectFile_returns_filename()
        => Fixture.Registry.Lookup("MSBuildProjectFile").Should().Be("App.csproj");

    [Test]
    public void MSBuildProjectDirectory_returns_directory_without_trailing_separator()
        => Fixture.Registry.Lookup("MSBuildProjectDirectory").Should().Be(Fixture.ProjectDir);

    [Test]
    public void MSBuildProjectExtension_returns_extension_with_dot()
        => Fixture.Registry.Lookup("MSBuildProjectExtension").Should().Be(".csproj");

    [Test]
    public void MSBuildProjectFullPath_returns_full_path()
        => Fixture.Registry.Lookup("MSBuildProjectFullPath").Should().Be(@"C:\repo\src\App\App.csproj");

    [Test]
    public void MSBuildProjectName_returns_name_without_extension()
        => Fixture.Registry.Lookup("MSBuildProjectName").Should().Be("App");

#if Is_Windows
    [Test]
    public void MSBuildProjectDirectoryNoRoot_strips_drive_root_without_trailing_separator()
        => Fixture.Registry.Lookup("MSBuildProjectDirectoryNoRoot").Should().Be(@"repo\src\App");
#endif
}

public class Lookup_distinguishes_anchors_when_files_differ
{
    private static readonly IOFile ContainingFile = IOFile.Parse(@"C:\repo\Directory.Build.props");
    private static readonly IOFile EntryFile = IOFile.Parse(@"C:\repo\src\App\App.csproj");
    private static readonly PropertyRegistry Registry = new(ContainingFile, EntryFile);

    [Test]
    public void This_file_directory_anchors_to_containing_file()
        => Registry.Lookup("MSBuildThisFileDirectory")
            .Should().Be($@"C:\repo{IOPath.Separator}");

    [Test]
    public void Project_directory_anchors_to_entry_file()
        => Registry.Lookup("MSBuildProjectDirectory")
            .Should().Be(@"C:\repo\src\App");

    [Test]
    public void This_file_name_uses_containing_file()
        => Registry.Lookup("MSBuildThisFile").Should().Be("Directory.Build.props");

    [Test]
    public void Project_file_name_uses_entry_file()
        => Registry.Lookup("MSBuildProjectFile").Should().Be("App.csproj");
}

public class Lookup_is_case_insensitive
{
    [Test]
    public void Lower_case_property_name_resolves()
        => Fixture.Registry.Lookup("msbuildthisfile").Should().Be("Lib.csproj");

    [Test]
    public void Upper_case_property_name_resolves()
        => Fixture.Registry.Lookup("MSBUILDTHISFILE").Should().Be("Lib.csproj");

    [Test]
    public void Mixed_case_property_name_resolves()
        => Fixture.Registry.Lookup("MsBuildProjectName").Should().Be("App");
}

public class Lookup_unknown_property
{
    [Test]
    public void Returns_null_for_user_defined_name_when_no_lookup_provided()
        => Fixture.Registry.Lookup("Configuration").Should().BeNull();

    [Test]
    public void Returns_null_for_arbitrary_name_when_no_lookup_provided()
        => Fixture.Registry.Lookup("NotReserved").Should().BeNull();

    [Test]
    public void Returns_null_for_empty_name()
        => Fixture.Registry.Lookup(string.Empty).Should().BeNull();
}

public class Lookup_user_defined
{
    [Test]
    public void Resolves_via_user_defined_lookup_when_property_not_reserved()
    {
        var registry = new PropertyRegistry(
            Fixture.ContainingFile,
            Fixture.EntryFile,
            name => name == "MyCustomProperty" ? "custom-value" : null);

        registry.Lookup("MyCustomProperty").Should().Be("custom-value");
    }

    [Test]
    public void Reserved_property_takes_precedence_over_user_defined_lookup()
    {
        var registry = new PropertyRegistry(
            Fixture.ContainingFile,
            Fixture.EntryFile,
            name => name == "MSBuildProjectName" ? "USER_OVERRIDE" : null);

        registry.Lookup("MSBuildProjectName").Should().Be("App");
    }

    [Test]
    public void User_defined_lookup_returning_null_yields_null_overall()
    {
        var registry = new PropertyRegistry(
            Fixture.ContainingFile,
            Fixture.EntryFile,
            name => null);

        registry.Lookup("Configuration").Should().BeNull();
    }

    [Test]
    public void User_defined_lookup_is_called_with_exact_name()
    {
        string? captured = null;
        var registry = new PropertyRegistry(
            Fixture.ContainingFile,
            Fixture.EntryFile,
            name => { captured = name; return null; });

        registry.Lookup("MyProperty");

        captured.Should().Be("MyProperty");
    }
}

public class Lookup_with_empty_files
{
    private static readonly PropertyRegistry Registry = new(IOFile.Empty, IOFile.Empty);

    [Test]
    public void Returns_empty_for_this_file_directory()
        => Registry.Lookup("MSBuildThisFileDirectory").Should().Be(string.Empty);

    [Test]
    public void Returns_empty_for_project_directory()
        => Registry.Lookup("MSBuildProjectDirectory").Should().Be(string.Empty);

    [Test]
    public void Returns_empty_for_this_file_directory_no_root()
        => Registry.Lookup("MSBuildThisFileDirectoryNoRoot").Should().Be(string.Empty);

    [Test]
    public void Returns_empty_for_project_directory_no_root()
        => Registry.Lookup("MSBuildProjectDirectoryNoRoot").Should().Be(string.Empty);
}

public class Lookup_no_root_with_relative_path
{
    private static readonly PropertyRegistry Registry = new(
        IOFile.Parse("relative/Lib.csproj"),
        IOFile.Parse("relative/App.csproj"));

    [Test]
    public void This_file_directory_no_root_returns_directory_unchanged_when_no_root_present()
        => Registry.Lookup("MSBuildThisFileDirectoryNoRoot")
            .Should().Be("relative" + IOPath.Separator);

    [Test]
    public void Project_directory_no_root_returns_directory_unchanged_when_no_root_present()
        => Registry.Lookup("MSBuildProjectDirectoryNoRoot").Should().Be("relative");
}

public class Substitute_known_properties
{
    [Test]
    public void Single_at_start()
    {
        var result = Fixture.Registry.Substitute("$(MSBuildThisFileDirectory)Foo.csproj");

        result.Substituted.Should().Be(Fixture.ThisDir + "Foo.csproj");
        result.Unresolved.Should().BeEmpty();
    }

    [Test]
    public void Single_in_middle()
    {
        var result = Fixture.Registry.Substitute("prefix-$(MSBuildProjectName)-suffix");

        result.Substituted.Should().Be("prefix-App-suffix");
        result.Unresolved.Should().BeEmpty();
    }

    [Test]
    public void Single_at_end()
    {
        var result = Fixture.Registry.Substitute("path/$(MSBuildProjectFile)");

        result.Substituted.Should().Be("path/App.csproj");
        result.Unresolved.Should().BeEmpty();
    }

    [Test]
    public void Multiple_separated_properties()
    {
        var result = Fixture.Registry.Substitute(
            "$(MSBuildThisFileDirectory)..\\$(MSBuildProjectName)\\file.txt");

        result.Substituted.Should().Be(Fixture.ThisDir + "..\\App\\file.txt");
        result.Unresolved.Should().BeEmpty();
    }

    [Test]
    public void Adjacent_properties()
    {
        var result = Fixture.Registry.Substitute(
            "$(MSBuildProjectName)$(MSBuildProjectExtension)");

        result.Substituted.Should().Be("App.csproj");
        result.Unresolved.Should().BeEmpty();
    }

    [Test]
    public void Same_property_twice_is_replaced_each_time()
    {
        var result = Fixture.Registry.Substitute(
            "$(MSBuildProjectName)/$(MSBuildProjectName)");

        result.Substituted.Should().Be("App/App");
        result.Unresolved.Should().BeEmpty();
    }

    [Test]
    public void Lower_case_property_reference_is_replaced()
    {
        var result = Fixture.Registry.Substitute("$(msbuildprojectname)");

        result.Substituted.Should().Be("App");
        result.Unresolved.Should().BeEmpty();
    }

    [Test]
    public void Mixed_case_property_reference_is_replaced()
    {
        var result = Fixture.Registry.Substitute("$(MsBuildProjectFile)");

        result.Substituted.Should().Be("App.csproj");
        result.Unresolved.Should().BeEmpty();
    }
}

public class Substitute_reports_unresolved
{
    [Test]
    public void Unknown_property_is_left_literal()
    {
        var result = Fixture.Registry.Substitute("$(NotReserved)/file.txt");

        result.Substituted.Should().Be("$(NotReserved)/file.txt");
    }

    [Test]
    public void Unknown_property_appears_in_unresolved_list()
    {
        var result = Fixture.Registry.Substitute("$(NotReserved)/file.txt");

        result.Unresolved.Should().BeEquivalentTo("NotReserved");
    }

    [Test]
    public void Mixed_known_and_unknown_replaces_only_known()
    {
        var result = Fixture.Registry.Substitute(
            "$(MSBuildProjectDirectory)/$(Configuration)/file.txt");

        result.Substituted.Should().Be(Fixture.ProjectDir + "/$(Configuration)/file.txt");
        result.Unresolved.Should().BeEquivalentTo("Configuration");
    }

    [Test]
    public void Multiple_unresolved_distinct_names_are_collected()
    {
        var result = Fixture.Registry.Substitute("$(Foo)/$(Bar)/$(Baz)");

        result.Unresolved.Should().BeEquivalentTo("Foo", "Bar", "Baz");
    }

    [Test]
    public void Same_unresolved_property_twice_is_listed_once()
    {
        var result = Fixture.Registry.Substitute("$(Configuration)/$(Configuration)");

        result.Unresolved.Should().BeEquivalentTo("Configuration");
    }

    [Test]
    public void Unresolved_dedups_case_insensitively()
    {
        var result = Fixture.Registry.Substitute("$(Configuration)/$(CONFIGURATION)/$(configuration)");

        result.Unresolved.Should().HaveCount(1);
    }
}

public class Substitute_edge_cases
{
    [Test]
    public void Empty_input_returns_empty()
    {
        var result = Fixture.Registry.Substitute(string.Empty);

        result.Substituted.Should().BeEmpty();
        result.Unresolved.Should().BeEmpty();
    }

    [Test]
    public void No_property_references_returns_input_unchanged()
    {
        var result = Fixture.Registry.Substitute("plain/path/file.txt");

        result.Substituted.Should().Be("plain/path/file.txt");
        result.Unresolved.Should().BeEmpty();
    }

    [Test]
    public void Missing_close_paren_passes_through_as_literal()
    {
        var result = Fixture.Registry.Substitute("$(MSBuildProjectName/file.txt");

        result.Substituted.Should().Be("$(MSBuildProjectName/file.txt");
        result.Unresolved.Should().BeEmpty();
    }

    [Test]
    public void Empty_property_name_passes_through_as_literal()
    {
        var result = Fixture.Registry.Substitute("$()/file.txt");

        result.Substituted.Should().Be("$()/file.txt");
        result.Unresolved.Should().BeEmpty();
    }

    [Test]
    public void Dotted_property_name_passes_through_as_literal()
    {
        var result = Fixture.Registry.Substitute("$(Foo.Bar)/file.txt");

        result.Substituted.Should().Be("$(Foo.Bar)/file.txt");
        result.Unresolved.Should().BeEmpty();
    }

    [Test]
    public void No_recursive_expansion()
    {
        var registry = new PropertyRegistry(
            IOFile.Parse(@"C:\$(Foo)\Lib.csproj"),
            IOFile.Parse(@"C:\repo\App.csproj"));

        var result = registry.Substitute("$(MSBuildThisFileDirectory)file.txt");

        result.Substituted.Should().StartWith(@"C:\$(Foo)\");
        result.Unresolved.Should().BeEmpty();
    }
}

public class Substitute_uses_user_defined_lookup
{
    [Test]
    public void Substitutes_user_defined_property()
    {
        var registry = new PropertyRegistry(
            Fixture.ContainingFile,
            Fixture.EntryFile,
            name => name == "Configuration" ? "Debug" : null);

        var result = registry.Substitute("path/$(Configuration)/file.txt");

        result.Substituted.Should().Be("path/Debug/file.txt");
        result.Unresolved.Should().BeEmpty();
    }

    [Test]
    public void Mixes_reserved_and_user_defined_in_one_input()
    {
        var registry = new PropertyRegistry(
            Fixture.ContainingFile,
            Fixture.EntryFile,
            name => name switch { "Configuration" => "Release", _ => null });

        var result = registry.Substitute("$(MSBuildProjectDirectory)/$(Configuration)/file.txt");

        result.Substituted.Should().Be(Fixture.ProjectDir + "/Release/file.txt");
        result.Unresolved.Should().BeEmpty();
    }

    [Test]
    public void Unknown_property_still_unresolved_when_lookup_returns_null()
    {
        var registry = new PropertyRegistry(
            Fixture.ContainingFile,
            Fixture.EntryFile,
            name => null);

        var result = registry.Substitute("$(Unknown)/file.txt");

        result.Substituted.Should().Be("$(Unknown)/file.txt");
        result.Unresolved.Should().BeEquivalentTo("Unknown");
    }
}
