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
    }

    public struct Color
    {
        public string HexValue;

        public Color(string hex)
        {
            HexValue = hex;
        }
    }

    //=============ENUMS=============

    public enum FlexDirection
    {
        Column = 0,
        ColumnReverse = 1,
        Row = 2,
        RowReverse
    }

    public enum LayoutDirection
    {
        Inherit = 0,
        LTR = 1,
        RTL = 2
    }

    public enum Justify
    {
        FlexStart = 0,
        Center = 1,
        FlexEnd = 2,
        SpaceBetween = 3,
        SpaceAround = 4,
        SpaceEvenly = 5
    }

    public enum Alignment
    {
        Auto = 0,
        FlexStart = 1,
        Center = 2,
        FlexEnd = 3,
        Stretch = 4,
        Baseline = 5,
        SpaceBetween = 6,
        SpaceAround = 7
    }

    public enum WrapMode
    {
        NoWrap = 0,
        Wrap = 1,
        WrapReverse = 2
    }

    //===============================

    internal struct ComponentNodePair
    {
        public Component Component;
        public YogaNode Node;
    }
}
