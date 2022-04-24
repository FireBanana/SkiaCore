using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiaCore.Slang
{
    public class SlangReader
    {
        private readonly string       _source;
        private readonly SlangScanner _scanner;

        private List<SlangToken>  _tokens;

        private bool _hasError = false;

        public SlangReader(string src, SlangScanner scanner)
        {
            _source = src;
            _scanner = scanner;        
        }

        private void RunFile(string path)
        {
            // Read file
            Run("");

            if (_hasError) Console.WriteLine("Has Error");
        }

        public void RunPrompt(string prompt)
        {
            Run(prompt);

            _hasError = false;
        }

        private void Run(string src)
        {
            _tokens = _scanner.ScanTokens();

            foreach (var token in _tokens)
                Console.WriteLine(token.ToString());
        }

        private void Report(int line, string loc, string mssg)
        {
            Console.WriteLine($"Error: {line}, {loc}, {mssg}");
        }
    }
}
