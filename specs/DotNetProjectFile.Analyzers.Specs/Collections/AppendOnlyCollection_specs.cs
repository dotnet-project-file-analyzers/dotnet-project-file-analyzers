using DotNetProjectFile.Collections;

namespace Collections.Immutable_Collection_specs;

public class Empty_collection
{
    [Test]
    public void Has_size_of_0()
        => AppendOnlyCollection<object>.Empty.Should().BeEmpty();

    [Test]
    public void Contains_no_items()
        => AppendOnlyCollection<object>.Empty.Should().BeEquivalentTo(Array.Empty<object>());

    [Test]
    public void Add_item_increases_the_size()
        => AppendOnlyCollection<object>.Empty.Add(new()).Should().ContainSingle();
}

public class Non_empty_collection
{
    [Test]
    public void Add_item_increases_the_size()
        => AppendOnlyCollection<object>.Empty
            .Add(new())
            .Add(new())
        .Should().HaveCount(2);
}

public class Add
{
    [Test]
    public void increases_the_size()
        => AppendOnlyCollection<object>.Empty.Add(new()).Should().ContainSingle();

    [Test]
    public void Null_item_has_no_effect()
        => AppendOnlyCollection<object>.Empty.Add(null!).Should().BeEmpty();

    [Test]
    public void Null_items_are_ignored()
        => AppendOnlyCollection<object>.Empty.AddRange(null!, 1, null!, 2, 3, null!, 4)
        .Should().BeEquivalentTo([1, 2, 3, 4]);

    [Test]
    public void Does_not_effect_shared_subs()
    {
        var parent = AppendOnlyCollection<object>.Empty.AddRange(1, 2);
        var first = parent.AddRange(4, 8, 16);
        var second = parent.AddRange(3, 4, 5);

        parent.Should().BeEquivalentTo([1, 2]);
        first.Should().BeEquivalentTo([1, 2, 4, 8, 16]);
        second.Should().BeEquivalentTo([1, 2, 3, 4, 5]);
    }
}

public class Enumerates
{
    [Test]
    public void are_added_as_fixed_list()
    {
        var items = AppendOnlyCollection<object>.Empty.Add(new());
        items.Should().ContainSingle(because: "the initial count is 1.");
        items.Should().ContainSingle(because: "calling it multiple times should not change a thing.");
    }
}
