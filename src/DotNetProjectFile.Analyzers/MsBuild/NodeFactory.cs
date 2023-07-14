using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using ConstructorFunc = System.Func<System.Xml.Linq.XElement, DotNetProjectFile.MsBuild.Node, DotNetProjectFile.MsBuild.Project, DotNetProjectFile.MsBuild.Node>;

namespace DotNetProjectFile.MsBuild;

/// <summary>
/// Used for creating instances of the <see cref="Node"/> class
/// based on given <see cref="XElement"/> instances.
/// </summary>
public static class NodeFactory
{
    private static readonly Type[] constructorArgumentTypes = GetConstructorParameterTypes();
    private static readonly ParameterExpression[] constructorArgumentExpressions = constructorArgumentTypes.Select(x => Expression.Parameter(x)).ToArray();
    private static readonly IReadOnlyDictionary<string, ConstructorFunc> constructors = BuildConstructorMap();

    public static Node? Create(XElement element, Node parent, Project project)
    {
        var name = element.Name.LocalName;

        if (name is null)
        {
            return null;
        }

        if (constructors.TryGetValue(name, out var constructor))
        {
            return constructor(element, parent, project);
        }

        return new Unknown(element, parent, project);
    }

    private static Type[] GetConstructorParameterTypes()
    {
        var all = typeof(ConstructorFunc).GenericTypeArguments;
        var result = new Type[all.Length - 1];
        Array.Copy(all, result, result.Length);
        return result;
    }

    private static Dictionary<string, ConstructorFunc> BuildConstructorMap()
        => typeof(Node).Assembly
        .GetTypes()
        .Select(GetValidNodeConstructor)
        .OfType<ConstructorInfo>()
        .ToDictionary(ci => ci.DeclaringType.Name, GenerateConstructor);

    private static ConstructorInfo? GetValidNodeConstructor(Type type)
    {
        if (type.IsAbstract || !typeof(Node).IsAssignableFrom(type))
        {
            return null;
        }

        return type.GetConstructor(constructorArgumentTypes);
    }

    private static ConstructorFunc GenerateConstructor(ConstructorInfo ci)
    {
        var args = constructorArgumentExpressions;
        var create = Expression.New(ci, args);
        var lambda = Expression.Lambda(create, args);
        return (ConstructorFunc)lambda.Compile();
    }
}
