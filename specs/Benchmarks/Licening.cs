using System.Text;

namespace Benchmarks;

public static class Licening
{
    public class PrepareText
    {
        private readonly string Text = @"MIT License

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

        [Benchmark]
        public string StringBuilder()
        {
            var sb = new StringBuilder();
            foreach (var ch in Text)
            {
                if (char.IsLetter(ch))
                {
                    sb.Append(char.ToLowerInvariant(ch));
                }
            }
            return sb.ToString();
        }

        [Benchmark]
        public string StringBuilder_Length()
        {
            var sb = new StringBuilder(Text.Length);
            foreach (var ch in Text)
            {
                if (char.IsLetter(ch))
                {
                    sb.Append(char.ToLowerInvariant(ch));
                }
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
            foreach(var ch in Text.Where(char.IsLetter).Select(char.ToLowerInvariant))
            {
                buffer[length++] = ch;
            }
            return new string(buffer, 0, length);
        }
    }
}
