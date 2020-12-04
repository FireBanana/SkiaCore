using SkiaSharp;

namespace SkiaCore.Components
{
    public abstract class InteractableComponent : Component
    {
        public abstract void OnClick();
        public abstract void OnMouseEnter();
        public abstract void OnMouseExit();

        public InteractableComponent(SKSurface surface, int x, int y, int width, int height, params object[] args) : base(surface, x, y, width, height, args)
        {
            
        }
    }
}
