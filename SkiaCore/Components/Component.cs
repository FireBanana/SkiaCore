using SkiaSharp;
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
            get { return (int)_node.Width.Value; }
            set { _node.Width = value; }
        }
        public int Height
        {
            get { return (int)_node.Height.Value; }
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
            => _paint.Color = SKColor.Parse(color.HexValue);

        public void SetAntiAlias(bool isAA)
            => _paint.IsAntialias = isAA;

        public void SetBorderSize(int size)
            => _bordersize = new SKSize(size, size);

        public void SetFlexDirection(FlexDirection dir) 
            => _node.FlexDirection = (YogaFlexDirection)dir;

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
                    X + _node.Width.Value,
                    Y + _node.Height.Value
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
