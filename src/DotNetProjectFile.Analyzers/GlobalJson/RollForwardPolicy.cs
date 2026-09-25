namespace DotNetProjectFile.GlobalJson;

public enum RollForwardPolicy
{
    None = 0,

    Patch,

    Feature,

    Minor,

    Major,

    LatestPatch,

    LatestFeature,

    LatestMinor,

    LatestMajor,

    Disable,
}
