using SkiaSharp;

namespace SkiaCore.Components
{
    public interface IComponent
    {
        SKSurface Surface { get; set; }
        int X { get; set; }
        int Y { get; set; }
        int Width { get; set; }
        int Height { get; set; }

        public void Initialize(SKSurface surface, int x, int y, int width, int height, params object[] args);
        public void Render();
    }
}
