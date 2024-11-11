using System.Linq.Expressions;
using System.Reflection;
using CtorFunc = System.Func<System.Xml.Linq.XElement, DotNetProjectFile.MsBuild.Node, DotNetProjectFile.MsBuild.Project, DotNetProjectFile.MsBuild.Node>;

namespace DotNetProjectFile.MsBuild;

/// <summary>
/// Used for creating instances of the <see cref="Node"/> class
/// based on given <see cref="XElement"/> instances.
/// </summary>
public sealed class NodeFactory
{
    internal NodeFactory()
    {
        CtorArgumentTypes = GetCtorParameterTypes();
        CtorArgumentExpressions = CtorArgumentTypes.Select(x => Expression.Parameter(x)).ToArray();
        Map = BuildCtorMap();
    }

    private readonly Type[] CtorArgumentTypes;
    private readonly ParameterExpression[] CtorArgumentExpressions;
    private readonly IReadOnlyDictionary<string, CtorFunc> Map;

    public Node? Create(XElement element, Node parent, MsBuildProject project)
        => element.Name.LocalName switch
        {
            null /*.......................................*/ => null,
            var n when Map.TryGetValue(n, out var con) /*.*/ => con(element, parent, project),
            _ /*..........................................*/ => new Unknown(element, parent, project),
        };

    public IReadOnlyCollection<string> KnownNodes => ((Dictionary<string, CtorFunc>)Map).Keys;

    private static Type[] GetCtorParameterTypes()
    {
        var all = typeof(CtorFunc).GenericTypeArguments;
        var result = new Type[all.Length - 1];
        Array.Copy(all, result, result.Length);
        return result;
    }

    private Dictionary<string, CtorFunc> BuildCtorMap()
        => typeof(Node).Assembly
        .GetTypes()
        .Select(GetValidNodeCtor)
        .OfType<ConstructorInfo>()
        .ToDictionary(ci => ci.DeclaringType.Name, GenerateCtor);

    private ConstructorInfo? GetValidNodeCtor(Type type)
        => type.IsAbstract || !typeof(Node).IsAssignableFrom(type)
            ? null
            : type.GetConstructor(CtorArgumentTypes);

    private CtorFunc GenerateCtor(ConstructorInfo ci)
    {
        var args = CtorArgumentExpressions;
        var create = Expression.New(ci, args);
        var lambda = Expression.Lambda(create, args);
        return (CtorFunc)lambda.Compile();
    }
}
