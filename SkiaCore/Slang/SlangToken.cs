using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiaCore.Slang
{
    public class SlangToken
    {
        private readonly SlangTokenType.TokenType _tokenType;
        private readonly string _lexeme;
        private readonly object _literal; //change
        private readonly int _line;

        public SlangToken(SlangTokenType.TokenType tokenType, string lexeme, object literal, int line)
        {
            _tokenType = tokenType;
            _lexeme = lexeme;
            _literal = literal;
            _line = line;
        }

        public override string ToString()
        {
            return _tokenType + " " + _lexeme + " " + _literal;
        }
    }
}
