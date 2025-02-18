using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetProjectFile.Licensing;

public sealed record SpdxLicenseInfo
{
    public required string Name { get; init; }

    public required string Id { get; init; }

    public required bool Osi { get; init; }

    public required bool Fsf { get; init; }

    public required ImmutableArray<string> SeeAlso { get; init; }

    public required string? LicenseText { get; init; }
}
