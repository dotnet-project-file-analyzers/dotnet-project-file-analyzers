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

public static class RollForwardPolicyExtensions
{
    extension(RollForwardPolicy policy)
    {
        /// <summary>Policies <see cref="RollForwardPolicy.Disable" />,
        /// <see cref="RollForwardPolicy.Patch"/>, and
        /// <see cref="RollForwardPolicy.LatestPatch"/> are considered strict.
        /// </summary>
        public bool IsStrict => policy
            is RollForwardPolicy.Disable
            or RollForwardPolicy.Patch
            or RollForwardPolicy.LatestPatch;
    }
}
