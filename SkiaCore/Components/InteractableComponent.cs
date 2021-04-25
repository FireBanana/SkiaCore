using SkiaSharp;

namespace SkiaCore.Components
{
    public abstract class InteractableComponent : Component
    {
        public abstract void OnClick();
        public abstract void OnRelease();
        public abstract void OnMouseEnter();
        public abstract void OnMouseExit();
        public virtual void OnKeyPress(string key) { }

        internal bool IsUnderCursor(int x, int y)
        {
            return CheckIfInsideComponent(x, y);
        }

        bool CheckIfInsideComponent(int mouseX, int mouseY)
        {
            if (mouseX > X &&
                mouseX < X + Width &&
                mouseY > Y &&
                mouseY < Y + Height)
                return true;
            else
                return false;
        }

        public InteractableComponent(SKSurface surface, int width, int height, params object[] args) : base(surface, width, height, args)
        {
            
        }
    }
}
