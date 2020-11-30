using SkiaSharp;

namespace SkiaCore.Components
{
    public interface IComponent
    {
        SKSurface _surface { get; set; }
        public void Initialize(SKSurface surface, params object[] args);
        public void Render();
    }
}
