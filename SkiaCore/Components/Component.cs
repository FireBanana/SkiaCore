using SkiaSharp;
using System.Diagnostics;
using Facebook.Yoga;
using System;
using SkiaCore.Common;

namespace SkiaCore.Components
{
    public abstract class Component
    {
        public int Id { get; internal set; }
        public int X  { get; internal set; }
        public int Y  { get; internal set; }

        internal bool WasMouseIn { get; set; }

        private YogaNode _node { get; set; }
        private SKPaint _paint { get; set; }
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
            
            Id = ++Core.IdCounter;
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
        }

        internal void CalculateLayout()
        {
            _node.CalculateLayout();
        }


    }
}
