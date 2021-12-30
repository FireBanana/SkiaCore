using Arqan;
using SkiaCore.Components;
using SkiaSharp;
using System.Collections.Generic;
using Facebook.Yoga;

namespace SkiaCore
{
    internal static class GraphicsRenderer
    {
        internal static SKSurface Surface;

        private static List<Component> _components = new List<Component>();
        private static SKColor _backgroundColor;
        private static RootComponent _root;

        public static RootComponent GetRoot() => _root;

        internal static void Initialize(SKSurface surface, int width, int height, SKColor color = default(SKColor))
        {
            Surface = surface;
            _backgroundColor = color;

            _root = new RootComponent(width, height);

            _components.Add(_root);
        }

        internal static void Update()
        {
            Surface.Canvas.Clear(_backgroundColor);

            foreach (var component in _components)
                component.Render(Surface);

            GL10.glTexImage2D(GL11.GL_TEXTURE_2D, 0, GL11.GL_RGB, Core.Width, Core.Height, 0, GL12.GL_BGRA, GL11.GL_UNSIGNED_BYTE, Surface.PeekPixels().GetPixels());
        }

        internal static void AddComponent(Component component)
        {
            _components.Add(component);
        }
    }
}
