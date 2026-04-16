using DotNetProjectFile.Caching;
using DotNetProjectFile.IO;
using System.IO;
using VerifyTests;

namespace Specs.Caching.FileCache_specs;

public class TryGetOrUpdate
{
    [Test]
    public async Task Adds_new_entry()
    {
        using var file = await TempFile.CreateText("Hello, World!");
        var cache = new FileCache<Entry>();
        cache.TryGetOrUpdate(IOFile.Parse(file.Path), p => new Entry());
        cache.Count.Should().Be(1);
    }

    [Test]
    public void Skips_empty_paths()
    {
        var cache = new FileCache<Entry>();
        cache.TryGetOrUpdate(IOFile.Empty, p => new Entry());
        cache.Count.Should().Be(0);
    }

    [Test]
    public async Task Keeps_existing_entry_when_file_has_same_LastWriteTimeUtc()
    {
        using var file = await TempFile.CreateText("Hello, World!");

        var cache = new FileCache<Entry>();
        cache.TryGetOrUpdate(IOFile.Parse(file.Path), p => new Entry());
        cache.Count.Should().Be(1);

        var result = cache.TryGetOrUpdate(IOFile.Parse(file.Path), p => new Entry(17));
        cache.Count.Should().Be(1);
        result.Should().Be(new Entry(42));
    }

    [Test]
    public async Task Updates_existing_entry()
    {
        using var file = await TempFile.CreateText("Hello, World!");

        var cache = new FileCache<Entry>();
        cache.TryGetOrUpdate(IOFile.Parse(file.Path), p => new Entry());
        cache.Count.Should().Be(1);

        using var writer = File.AppendText(file.Path);
        await writer.WriteAsync('.');
        await writer.FlushAsync();

        var result = cache.TryGetOrUpdate(IOFile.Parse(file.Path), p => new Entry(17));
        cache.Count.Should().Be(1);
        result.Should().Be(new Entry(17));
    }

    [Test]
    public async Task Remove_existing_entry_when_transform_fails()
    {
        using var file = await TempFile.CreateText("Hello, World!");

        var cache = new FileCache<Entry>();
        cache.TryGetOrUpdate(IOFile.Parse(file.Path), p => new Entry());
        cache.Count.Should().Be(1);

        using var writer = File.AppendText(file.Path);
        await writer.WriteAsync('.');
        await writer.FlushAsync();

        var result = cache.TryGetOrUpdate(IOFile.Parse(file.Path), p => null);
        cache.Count.Should().Be(0);
        result.Should().BeNull();
    }
}

file record Entry(int Value = 42);
