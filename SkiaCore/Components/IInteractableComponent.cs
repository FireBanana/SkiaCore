using SkiaSharp;

namespace SkiaCore.Components
{
    public interface IInteractableComponent : IComponent
    {
        void OnClick();
        void OnMouseEnter();
        void OnMouseExit();
    }
}
