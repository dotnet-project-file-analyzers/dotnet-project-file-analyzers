using FluentAssertions;
using System;

namespace PackagesWithoutAnalyzers;

public class Placeholder : Microsoft.AspNetCore.Mvc.ControllerBase
{
    [MongoDB.Bson.Serialization.Attributes.BsonId]
    public Qowaiv.Uuid Id { get; init; }

    [NUnit.Framework.Test]
    public void NUnit_Test() => Math.Sqrt(9).Should().Be(3);

    [Xunit.Fact]
    public void xunit_Test() => Math.Sqrt(4).Should().Be(2);

    public void Log() => Serilog.Log.Debug("Hello, world!");

    public void Fx(Microsoft.Azure.Functions.Extensions.DependencyInjection.FunctionsStartup startup) 
        => startup.Should().NotBeNull();

    public static Microsoft.EntityFrameworkCore.DbContext? Table() => null;
}
