namespace Specs.TestTools;

/// <summary>Decorate a assertion method.</summary>
/// <remarks>
/// Sonar's S2699 reports on test methods lacking an assertion. To help the
/// analysis, by decoration methods that are not regonized as being assertions
/// automatically, the rule will not longer falsly flag those tests.
/// </remarks>
[AttributeUsage(AttributeTargets.Method)]
internal sealed class AssertionMethodAttribute : Attribute;
