using Arqan;
using SkiaCore.Components;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SkiaCore
{
    internal static class GraphicsRenderer
    {
        internal static SKSurface Surface;
        static List<IComponent> _components = new List<IComponent>();

        internal static void Initialize(SKSurface surface)
        {
            Surface = surface;
        }

        internal static void Update()
        {
            Surface.Canvas.Clear(SKColors.CadetBlue);

            foreach (var component in _components) component.Render();

            GL10.glTexImage2D(GL11.GL_TEXTURE_2D, 0, GL11.GL_RGB, Core.Width, Core.Height, 0, GL12.GL_BGRA, GL11.GL_UNSIGNED_BYTE, Surface.PeekPixels().GetPixels());
        }

        internal static void AddComponent(IComponent component)
        {
            _components.Add(component);
        }
    }
}
