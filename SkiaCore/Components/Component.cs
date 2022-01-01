using SkiaSharp;
using System.Diagnostics;
using Facebook.Yoga;
using System;

namespace SkiaCore.Components
{
    public abstract class Component
    {
        public int Id;
        public int X { get; internal set; }
        public int Y { get; internal set; }
        internal bool WasMouseIn { get; set; }

        private YogaNode _node { get; set; }
        private SKPaint _paint { get; set; }

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

        public void SetColor(SKColor color) => _paint.Color = color;

        public Component(int width, int height, params object[] args)
        {
            _node = new YogaNode();

            _node.Width = width;
            _node.Height = height;

            _paint = new SKPaint();
            
            Id = ++Core.IdCounter;
        }

        internal void Attach(Component child)
        {
            _node.AddChild(child._node);
        }

        internal void Render(SKSurface surface)
        {
            surface.Canvas.DrawRect(
                new SKRect(
                    X,
                    Y,
                    X + _node.Width.Value,
                    Y + _node.Height.Value
                    ),
                _paint
                );
        }

        internal void CalculateLayout()
        {
            _node.CalculateLayout();
        }


    }
}
