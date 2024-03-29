﻿using SkiaSharp;
using System.Diagnostics;
using Facebook.Yoga;
using System;
using SkiaCore.Common;
using System.Text;
using SkiaCore.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace SkiaCore.Components
{
    public abstract class Component
    {
        private readonly YogaNode _node;
        private readonly SKPaint _paint;

        private readonly List<IEffect> _effects;

        public int X  { get; internal set; }
        public int Y  { get; internal set; }

        internal bool WasMouseIn { get; set; }

        private SKSize _bordersize { get; set; }

        internal YogaNode GetNode() => _node;

        public int Width
        {
            get { return (int)_node.LayoutWidth; }
            set { _node.Width = value; }
        }
        public int Height
        {
            get { return (int)_node.LayoutHeight; }
            set { _node.Height = value; }
        }

        public Component(int width, int height, params object[] args)
        {
            _node = new YogaNode();

            _node.Width = width;
            _node.Height = height;

            _paint = new SKPaint();
            _effects = new List<IEffect>();
        }

        public void InsertEffect(IEffect effect)
        {
            _effects.Add(effect);
        }

        //================================= SET NODE SPECIFICS ======================================

        public void SetColor(Color color)
            => _paint.Color = color.RawColor;

        public void SetAntiAlias(bool isAA)
            => _paint.IsAntialias = isAA;

        public void SetBorderSize(int size)
            => _bordersize = new SKSize(size, size);

        public void SetFlexDirection(FlexDirection dir) 
            => _node.FlexDirection = (YogaFlexDirection)dir;

        public void SetFlexGrow(float grow)
            => _node.FlexGrow = grow;

        public void SetLayoutDirection(LayoutDirection lDir) 
            => _node.StyleDirection = (YogaDirection)lDir;

        public void SetJustifyContent(Justify just) 
            => _node.JustifyContent = (YogaJustify)just;

        public void SetWrapMode(WrapMode wMode)
            => _node.Wrap = (YogaWrap)wMode;

        public void SetAlignItems(Alignment align)
            => _node.AlignItems = (YogaAlign)align;

        public void SetAlignSelf(Alignment align)
            => _node.AlignSelf = (YogaAlign)align;

        public void SetAlignContent(Alignment align)
            => _node.AlignContent = (YogaAlign)align;

        public void SetPadding(float val)
            => _node.Padding = val;

        public void SetMargin(float val)
            => _node.Margin = val;

        public void SetMargin(float t, float r, float b, float l)
        {
            _node.MarginTop = t;
            _node.MarginRight = r;
            _node.MarginBottom = b;
            _node.MarginLeft = l;
        }

        public void SetHeightAuto()
            => _node.Height = YogaValue.Auto();

        public void SetWidthAuto()
            => _node.Width = YogaValue.Auto();

        //===========================================================================================

        internal void Attach(Component child)
        {
            _node.AddChild(child._node);
        }

        internal void Render(SKSurface surface)
        {
            surface.Canvas.DrawRoundRect(
                new SKRect(
                    X,
                    Y,
                    X + Width,
                    Y + Height
                    ),
                _bordersize,
                _paint
                );

            foreach (var e in _effects)
                e.Draw(this, surface.Canvas);
        }

        internal void CalculateLayout()
        {
            _node.CalculateLayout();
        }
    }
}
