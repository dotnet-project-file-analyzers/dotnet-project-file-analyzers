using System.Security.Cryptography;
using System.Text;

namespace Benchmarks;

public static class Licensing
{
    private static readonly string Text = @"MIT License

Copyright (c) <year> <copyright holders>

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
associated documentation files (the ""Software""), to deal in the Software without restriction, including 
without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the 
following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial 
portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT 
LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO 
EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER 
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE 
USE OR OTHER DEALINGS IN THE SOFTWARE.
";

    public class PrepareText
    {
        [Benchmark]
        public string StringBuilder()
        {
            var sb = new StringBuilder();
            foreach (var ch in Text.Where(char.IsLetter))
            {
                sb.Append(char.ToLowerInvariant(ch));
            }
            return sb.ToString();
        }

        [Benchmark]
        public string StringBuilder_Length()
        {
            var sb = new StringBuilder(Text.Length);
            foreach (var ch in Text.Where(char.IsLetter))
            {
                sb.Append(char.ToLowerInvariant(ch));
            }
            return sb.ToString();
        }

        [Benchmark]
        public string Concat()
            => string.Concat(Text.Where(char.IsLetter).Select(char.ToLowerInvariant));

        [Benchmark]
        public string Alt()
        {
            var buffer = new char[Text.Length];
            var length = 0;
            foreach (var ch in Text.Where(char.IsLetter).Select(char.ToLowerInvariant))
            {
                buffer[length++] = ch;
            }
            return new string(buffer, 0, length);
        }
    }

    [MemoryDiagnoser(true)]
    public class Hashing
    {
#pragma warning disable SYSLIB0021 // Type or member is obsolete: we use it to benchmark it
        private static readonly SHA1 sha1 = SHA1.Create();
        private static readonly SHA1 sha1_managed = SHA1Managed.Create();
        private static readonly IncrementalHash sha1_incremental = IncrementalHash.CreateHash(HashAlgorithmName.SHA1);
        private static readonly SHA256 sha256 = SHA256.Create();
        private static readonly SHA256 sha256_managed = SHA256Managed.Create();
        private static readonly IncrementalHash sha256_incremental = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        private static readonly IncrementalHash sha512_incremental = IncrementalHash.CreateHash(HashAlgorithmName.SHA512);
#pragma warning restore SYSLIB0021 // Type or member is obsolete

        [Params(1)]
        public int N { get; set; }

        [Benchmark]
        public string[] SHA1_Static()
        {
            var results = new string[N];
            for (var i = 0; i < N; i++)
            {
                results[i] = Convert.ToBase64String(SHA1.HashData(Encoding.UTF8.GetBytes(Text)));
            }

            return results;
        }

        [Benchmark]
        public string[] SHA1_Object()
        {
            var results = new string[N];
            for (var i = 0; i < N; i++)
            {
                results[i] = Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes(Text)));
            }

            return results;
        }

        [Benchmark]
        public string[] SHA1_Managed()
        {
            var results = new string[N];
            for (var i = 0; i < N; i++)
            {
                results[i] = Convert.ToBase64String(sha1_managed.ComputeHash(Encoding.UTF8.GetBytes(Text)));
            }

            return results;
        }

        [Benchmark]
        public string[] SHA1_Incremental()
        {
            var results = new string[N];
            for (var i = 0; i < N; i++)
            {
                sha1_incremental.AppendData(Encoding.UTF8.GetBytes(Text));
                results[i] = Convert.ToBase64String(sha1_incremental.GetHashAndReset());
            }

            return results;
        }

        [Benchmark]
        public string[] SHA256_Static()
        {
            var results = new string[N];
            for (var i = 0; i < N; i++)
            {
                results[i] = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(Text)));
            }

            return results;
        }

        [Benchmark]
        public string[] SHA256_Object()
        {
            var results = new string[N];
            for (var i = 0; i < N; i++)
            {
                results[i] = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(Text)));
            }

            return results;
        }

        [Benchmark]
        public string[] SHA256_Managed()
        {
            var results = new string[N];
            for (var i = 0; i < N; i++)
            {
                results[i] = Convert.ToBase64String(sha256_managed.ComputeHash(Encoding.UTF8.GetBytes(Text)));
            }

            return results;
        }

        [Benchmark]
        public string[] SHA256_Incremental()
        {
            var results = new string[N];
            for (var i = 0; i < N; i++)
            {
                sha256_incremental.AppendData(Encoding.UTF8.GetBytes(Text));
                results[i] = Convert.ToBase64String(sha256_incremental.GetHashAndReset());
            }

            return results;
        }

        [Benchmark]
        public string[] SHA256_Incremental_Truncated()
        {
            var results = new string[N];
            for (var i = 0; i < N; i++)
            {
                sha256_incremental.AppendData(Encoding.UTF8.GetBytes(Text));
                results[i] = Convert.ToBase64String(sha256_incremental.GetHashAndReset().AsSpan(0, 16).ToArray());
            }

            return results;
        }

        [Benchmark]
        public string[] SHA512_Incremental_Truncated()
        {
            var results = new string[N];
            for (var i = 0; i < N; i++)
            {
                sha256_incremental.AppendData(Encoding.UTF8.GetBytes(Text));
                results[i] = Convert.ToBase64String(sha512_incremental.GetHashAndReset().AsSpan(0, 16).ToArray());
            }

            return results;
        }
    }
}
