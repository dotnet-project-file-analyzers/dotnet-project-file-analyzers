using DotNetProjectFile.MsBuild.Conversion;
using System.ComponentModel;
using System.Reflection;

namespace Conversion.TypeConverters_specs;

public class Converts
{
    [Test]
    public void null_string_to_string_as_null()
        => TypeConverters.TryConvert<string?>(null).Should().BeNull();

    [Test]
    public void empty_string_to_string_as_null()
        => TypeConverters.TryConvert<string?>(string.Empty).Should().BeNull();

    [Test]
    public void null_to_nullable_bool_as_null()
        => TypeConverters.TryConvert<bool?>(null).Should().BeNull();

    [Test]
    public void null_to_bool_as_false()
        => TypeConverters.TryConvert<bool>(null).Should().Be(false);

    [Test]
    public void true_to_bool_as_true()
        => TypeConverters.TryConvert<bool>("true").Should().Be(true);
}


public class Registered
{
    private static IEnumerable<Type> Decorated
        => typeof(DotNetProjectFile.Analyzers.MsBuildProjectFileAnalyzer)
        .Assembly.GetTypes()
        .Where(t => t.GetCustomAttributes<TypeConverterAttribute>().Any());

    [TestCaseSource(nameof(Decorated))]
    public void For(Type type)
    {
        var attr = type.GetCustomAttributes<TypeConverterAttribute>().Single();
        attr.ConverterTypeName.Should().Be(TypeConverters.Get(type).GetType().AssemblyQualifiedName);
    }
}
