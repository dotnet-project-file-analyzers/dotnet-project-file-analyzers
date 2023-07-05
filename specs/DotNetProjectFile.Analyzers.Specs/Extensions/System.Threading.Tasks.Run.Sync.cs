namespace System.Threading.Tasks;

/// <remarks>
/// Microsoft built an AsyncHelper (internal) class to run Async as Sync.
/// This implementation is equal to that one, besides some naming.
/// </remarks>
internal static class Run
{
    [Pure]
    public static TResult Sync<TResult>(Func<Task<TResult>> task)
        => Factory
            .StartNew(task)
            .Unwrap()
            .GetAwaiter()
            .GetResult();

    public static void Sync(Func<Task> task)
        => Factory
            .StartNew(task)
            .Unwrap()
            .GetAwaiter()
            .GetResult();

    private static readonly TaskFactory Factory = new(
       CancellationToken.None,
       TaskCreationOptions.None,
       TaskContinuationOptions.None,
       TaskScheduler.Default);
}
