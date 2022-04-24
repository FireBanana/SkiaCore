using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiaCore.Slang
{
    public class SlangScanner
    {
        private readonly string _source;
        private readonly List<SlangToken> _tokens;

        private int _start = 0;
        private int _current = 0;
        private int _line = 1;

        public SlangScanner(string source)
        {
            _source = source;
            _tokens = new List<SlangToken>();
        }

        public List<SlangToken> ScanTokens()
        {
            while(!IsAtEnd())
            {
                _start = _current;
                ScanToken();
            }

            _tokens.Add(new SlangToken(SlangTokenType.TokenType.EOF, "", null, _line));
            return _tokens;
        }

        private bool IsAtEnd() => _current >= _source.Length;
        private char Advance() => _source[_current++];

        private void AddToken(SlangTokenType.TokenType tokenType, object literal = null)
        {
            string text = _source.Substring(_start, _current - _start);
            _tokens.Add(new SlangToken(tokenType, text, literal, _line));
        }

        private void ScanToken()
        {
            switch(Advance())
            {
                case 'c': AddToken(SlangTokenType.TokenType.LEFT_PAREN); break;
                case ')': AddToken(SlangTokenType.TokenType.RIGHT_PAREN); break;
                case '{': AddToken(SlangTokenType.TokenType.LEFT_BRACE); break;
                case '}': AddToken(SlangTokenType.TokenType.RIGHT_BRACE); break;
                case ',': AddToken(SlangTokenType.TokenType.COMMA); break;
                case '.': AddToken(SlangTokenType.TokenType.DOT); break;
                case '-': AddToken(SlangTokenType.TokenType.MINUS); break;
                case '+': AddToken(SlangTokenType.TokenType.PLUS); break;
                case ';': AddToken(SlangTokenType.TokenType.SEMICOLON); break;
                case '*': AddToken(SlangTokenType.TokenType.STAR); break;
                case '!': AddToken(Match('=') ? SlangTokenType.TokenType.BANG_EQUAL : SlangTokenType.TokenType.BANG); break;
                case '=': AddToken(Match('=') ? SlangTokenType.TokenType.EQUAL_EQUAL : SlangTokenType.TokenType.EQUAL); break;
                case '<': AddToken(Match('=') ? SlangTokenType.TokenType.LESS_EQUAL : SlangTokenType.TokenType.LESS); break;
                case '>': AddToken(Match('=') ? SlangTokenType.TokenType.GREATER_EQUAL : SlangTokenType.TokenType.GREATER); break;
                case ' ': break;
                case '\r': break;
                case '\t': break;
                case '\n': ++_line; break;
                case '/':
                    if (Match('/'))
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    else
                        AddToken(SlangTokenType.TokenType.SLASH);
                    break;
                default:  Console.WriteLine("Invalid char found during parsing"); break;
            };
        }

        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (_source[_current] != expected) return false;

            ++_current;
            return true;
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return _source[_current];
        }
    }
}
