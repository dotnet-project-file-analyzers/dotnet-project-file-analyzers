using FluentAssertions;
using System;

namespace PackagesWithAnalyzers;

public sealed class Placeholder : Microsoft.AspNetCore.Mvc.ControllerBase
{
    [MongoDB.Bson.Serialization.Attributes.BsonId]
    public Qowaiv.Uuid Id { get; init; }

    public static readonly Type[] GetAttributes = new[]
    {
        typeof(NUnit.Framework.TestAttribute),
        typeof(Xunit.FactAttribute),
    };

    public static void Log() => Serilog.Log.Debug("Hello, world!");

    public static void Fx(Microsoft.Azure.Functions.Extensions.DependencyInjection.FunctionsStartup startup)
        => startup.Should().NotBeNull();

    public static Microsoft.EntityFrameworkCore.DbContext? Table() => null;
}
