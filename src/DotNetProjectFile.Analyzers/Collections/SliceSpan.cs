namespace DotNetProjectFile.Collections;

public readonly record struct SliceSpan(int Start, int Size)
{
    public int End => Start + Size;

    public static SliceSpan operator +(SliceSpan left, SliceSpan right)
        => left == default
            ? right
            : new(left.Start, right.Size + right.Start - left.Start);
}
