using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using CtorFunc = System.Func<System.Xml.Linq.XElement, DotNetProjectFile.MsBuild.Node, DotNetProjectFile.MsBuild.Project, DotNetProjectFile.MsBuild.Node>;

namespace DotNetProjectFile.MsBuild;

/// <summary>
/// Used for creating instances of the <see cref="Node"/> class
/// based on given <see cref="XElement"/> instances.
/// </summary>
internal static class NodeFactory
{
    private static readonly Type[] ctorArgumentTypes = GetCtorParameterTypes();
    private static readonly ParameterExpression[] ctorArgumentExpressions = ctorArgumentTypes.Select(x => Expression.Parameter(x)).ToArray();
    private static readonly IReadOnlyDictionary<string, CtorFunc> map = BuildCtorMap();

    public static Node? Create(XElement element, Node parent, MsBuildProject project)
        => element.Name.LocalName switch
        {
            null /*.......................................*/ => null,
            var n when map.TryGetValue(n, out var con) /*.*/ => con(element, parent, project),
            _ /*..........................................*/ => new Unknown(element, parent, project),
        };

    public static IReadOnlyCollection<string> KnownNodes => ((Dictionary<string, CtorFunc>)map).Keys;

    private static Type[] GetCtorParameterTypes()
    {
        var all = typeof(CtorFunc).GenericTypeArguments;
        var result = new Type[all.Length - 1];
        Array.Copy(all, result, result.Length);
        return result;
    }

    private static Dictionary<string, CtorFunc> BuildCtorMap()
        => typeof(Node).Assembly
        .GetTypes()
        .Select(GetValidNodeCtor)
        .OfType<ConstructorInfo>()
        .ToDictionary(ci => ci.DeclaringType.Name, GenerateCtor);

    private static ConstructorInfo? GetValidNodeCtor(Type type)
        => type.IsAbstract || !typeof(Node).IsAssignableFrom(type)
            ? null
            : type.GetConstructor(ctorArgumentTypes);

    private static CtorFunc GenerateCtor(ConstructorInfo ci)
    {
        var args = ctorArgumentExpressions;
        var create = Expression.New(ci, args);
        var lambda = Expression.Lambda(create, args);
        return (CtorFunc)lambda.Compile();
    }
}
