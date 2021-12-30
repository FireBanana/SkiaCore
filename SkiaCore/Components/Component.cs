using SkiaSharp;
using System.Diagnostics;
using Facebook.Yoga;

namespace SkiaCore.Components
{
    public abstract class Component
    {
        public int Id;        
        public int X { get; set; }
        public int Y { get; set; }
        internal bool WasMouseIn { get; set; }
        private YogaNode _node { get; set; }

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
            
            Id = Core.IdCounter++;
        }

        public void Attach(Component child)
        {
            _node.AddChild(child._node);
        }

        public void Render(SKSurface surface)
        {
            surface.Canvas.DrawRect(
                new SKRect(
                    _node.LayoutX,
                    _node.LayoutY,
                    _node.LayoutX + _node.Width.Value,
                    _node.LayoutY + _node.Height.Value
                    ),
                new SKPaint()
                );
        }
    }
}
