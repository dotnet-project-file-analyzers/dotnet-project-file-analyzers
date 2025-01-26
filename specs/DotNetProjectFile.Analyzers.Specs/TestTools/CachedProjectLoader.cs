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
                        value = ProjectLoader.Load(file);
                    }

                    return value;
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
