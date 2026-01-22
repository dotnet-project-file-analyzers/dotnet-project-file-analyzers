using System.Linq.Expressions;
using System.Reflection;
using CtorFunc = System.Func<System.Xml.Linq.XElement, DotNetProjectFile.NuGet.Configuration.Node, DotNetProjectFile.NuGet.Configuration.NuGetConfigFile, DotNetProjectFile.NuGet.Configuration.Node>;

namespace DotNetProjectFile.NuGet.Configuration;

/// <summary>
/// Used for creating instances of the <see cref="Node"/> class
/// based on given <see cref="XElement"/> instances.
/// </summary>
internal sealed class NodeFactory
{
    public NodeFactory()
    {
        CtorArgumentTypes = GetCtorParameterTypes();
        CtorArgumentExpressions = [.. CtorArgumentTypes.Select(x => Expression.Parameter(x))];
        CaseInsensitive = BuildCtorMap();
    }

    private readonly Type[] CtorArgumentTypes;
    private readonly ParameterExpression[] CtorArgumentExpressions;
    private readonly Dictionary<string, CtorFunc> CaseInsensitive;

    [Pure]
    public Node Create(XElement element, Node parent, NuGetConfigFile configFile)
        => Lookup(element).TryGetValue(element.Name.LocalName, out var con)
        ? con(element, parent, configFile)
        : new Unknown(element, parent, configFile);

    [Pure]
    private Dictionary<string, CtorFunc> Lookup(XElement element) => CaseInsensitive;

    [Pure]
    private static Type[] GetCtorParameterTypes()
    {
        var all = typeof(CtorFunc).GenericTypeArguments;
        return [.. all.Take(all.Length - 1)];
    }

    [Pure]
    private Dictionary<string, CtorFunc> BuildCtorMap()
        => typeof(Node).Assembly
        .GetTypes()
        .Select(GetValidNodeCtor)
        .OfType<ConstructorInfo>()
        .ToDictionary(Key, GenerateCtor, StringComparer.OrdinalIgnoreCase);

    /// <summary>Lookup can not (easily) access the LocalName (override).</summary>
    private static string Key(ConstructorInfo ci) => ci.DeclaringType.Name switch
    {
        nameof(NuGet) => "configuration",
        var name => name,
    };

    [Pure]
    private ConstructorInfo? GetValidNodeCtor(Type type)
        => type.IsAbstract || !typeof(Node).IsAssignableFrom(type)
            ? null
            : type.GetConstructor(CtorArgumentTypes);

    [Pure]
    private CtorFunc GenerateCtor(ConstructorInfo ci)
    {
        var args = CtorArgumentExpressions;
        var create = Expression.New(ci, args);
        var lambda = Expression.Lambda(create, args);
        return (CtorFunc)lambda.Compile();
    }
}
