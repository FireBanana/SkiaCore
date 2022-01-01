using Facebook.Yoga;
using SkiaCore.Components;
using SkiaSharp;
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

    public struct SkiaCoreOptions
    {
        public bool IsBorderless;
        public bool IsNotResizable;
        public SKColor BackgroundColor;
    }

    internal struct ComponentNodePair
    {
        public Component Component;
        public YogaNode Node;
    }
}
