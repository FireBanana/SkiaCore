using SkiaSharp;
using System.Diagnostics;

namespace SkiaCore.Components
{
    public abstract class Component
    {
        public int Id;
        public SKSurface Surface { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        internal bool WasMouseIn { get; set; } //Check state of mouse in previous frame

        public abstract void Render();
        public Component(SKSurface surface, int width, int height, params object[] args)
        {
            Surface = surface;
            Width = width;
            Height = height;
            Id = Core.IdCounter++;
        }
    }
}
