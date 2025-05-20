using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace Specs.TestTools;

internal static class CachedProjectLoader
{
    private static readonly ConcurrentDictionary<string, Entry> projectCache = new();
    private static readonly Lock projectCacheLock = new();

    private sealed class Entry(FileInfo file)
    {
        // Used for exponential back-off when IO exceptions occur due to conflicts in file access.
        private readonly Random rnd = new(file.GetHashCode());
        
        private readonly Lock locker = new();
        private Project? value;

        public Project Value
        {
            get
            {
                if (value is { })
                {
                    return value;
                }

                lock (locker)
                {
                    if (value is not { })
                    {
                        value = TryLoad();
                    }

                    return value;
                }
            }
        }

        private Project TryLoad()
        {
            var waited = 0;

            // Retry a few times (with random back-off) to read the file.
            // Very rarely this fails due to conflicts. Most of the time,
            // it will succeed on first try.

            while (true)
            {
                try
                {
                    return ProjectLoader.Load(file);
                }
                catch (IOException)
                {
                    if (waited >= 10_000)
                    {
                        throw;
                    }

                    var wait = rnd.Next(1, 201);
                    waited += wait;
                    Thread.Sleep(wait);
                }
            }
        }
    }

    public static Project Load(FileInfo file)
    {
        var key = file.FullName;

        if (projectCache.TryGetValue(key, out var project))
        {
            return project.Value;
        }

        lock (projectCacheLock)
        {
            if (!projectCache.TryGetValue(key, out project))
            {
                project = new Entry(file);
                projectCache[key] = project;
            }
        }

        return project.Value;
    }
}
