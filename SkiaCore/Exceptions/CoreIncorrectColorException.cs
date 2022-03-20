using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiaCore.Exceptions
{
    public class CoreIncorrectColorException : Exception
    {
        public CoreIncorrectColorException(string message) : base(message)
        {

        }
    }
}
