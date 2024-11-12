using System.Linq.Expressions;
using System.Reflection;
using CtorFunc = System.Func<System.Xml.Linq.XElement, DotNetProjectFile.MsBuild.Node, DotNetProjectFile.MsBuild.Project, DotNetProjectFile.MsBuild.Node>;

namespace DotNetProjectFile.MsBuild;

/// <summary>
/// Used for creating instances of the <see cref="Node"/> class
/// based on given <see cref="XElement"/> instances.
/// </summary>
internal sealed class NodeFactory
{
    public NodeFactory()
    {
        CtorArgumentTypes = GetCtorParameterTypes();
        CtorArgumentExpressions = CtorArgumentTypes.Select(x => Expression.Parameter(x)).ToArray();
        Map = BuildCtorMap();
    }

    private readonly Type[] CtorArgumentTypes;
    private readonly ParameterExpression[] CtorArgumentExpressions;
    private readonly IReadOnlyDictionary<string, CtorFunc> Map;

    [Pure]
    public Node Create(XElement element, Node parent, MsBuildProject project)
        => Map.TryGetValue(element.Name.LocalName, out var con)
        ? con(element, parent, project)
        : new Unknown(element, parent, project);

    [Pure]
    private static Type[] GetCtorParameterTypes()
    {
        var all = typeof(CtorFunc).GenericTypeArguments;
        return all.Take(all.Length - 1).ToArray();
    }

    [Pure]
    private IReadOnlyDictionary<string, CtorFunc> BuildCtorMap()
        => typeof(Node).Assembly
        .GetTypes()
        .Select(GetValidNodeCtor)
        .OfType<ConstructorInfo>()
        .ToDictionary(ci => ci.DeclaringType.Name, GenerateCtor);

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
