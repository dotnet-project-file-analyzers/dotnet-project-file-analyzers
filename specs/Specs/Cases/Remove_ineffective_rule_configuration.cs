using System.Diagnostics.CodeAnalysis;

#pragma warning disable S101
#pragma warning disable CS1680 // Noncompliant {{Rule CS1680 is not-configurable and cannot be modified}}
//                      ^^^^^^

[assembly: SuppressMessage("Category", "CS0016")] // Noncompliant {{Rule CS0016 is not-configurable and cannot be modified}}
//                                     ^^^^^^^^

[SuppressMessage("Category", "SA1603:Documentation")] // Noncompliant {{Rule SA1603 is not-configurable and cannot be modified}}
//                           ^^^^^^^^^^^^^^^^^^^^^^
public class SomeClass
{
    private const string CS1680 = "CS1680";

    [SuppressMessage("Category", "QW0001")]
    [SuppressMessage(checkId: CS1680, category: "Category")] // Noncompliant
    //                        ^^^^^^
    public void SomeMethod() { }
}
