using System;
using System.Collections.Generic;
using System.Text;

namespace SkiaCore.Common
{
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
