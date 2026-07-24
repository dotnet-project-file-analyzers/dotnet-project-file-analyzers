namespace Specs.Grammr_Types_specs;

public class Namespaces
{
    [TestCaseSource(nameof(Types))]
    public void Have_Grammr_as_root(Type type)
        => type.Namespace.Should().StartWith("Grammr");

    private static IEnumerable<Type> Types => typeof(Grammr.Source)
        .Assembly.GetTypes()
        .Where(t => t.Namespace?.Contains("Grammr") is true);
}
