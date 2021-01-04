using Arqan;
using SkiaCore.Components;
using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SkiaCore
{
    internal static class GraphicsRenderer
    {
        static SKSurface Surface;
        static List<Component> _components = new List<Component>();
        static SKColor _backgroundColor;

        internal static void Initialize(SKSurface surface, SKColor color = default(SKColor))
        {
            Surface = surface;
            _backgroundColor = color;
        }

        internal static void Update()
        {
            Surface.Canvas.Clear(_backgroundColor);

            foreach (var component in _components) component.Render();

            GL10.glTexImage2D(GL11.GL_TEXTURE_2D, 0, GL11.GL_RGB, Core.Width, Core.Height, 0, GL12.GL_BGRA, GL11.GL_UNSIGNED_BYTE, Surface.PeekPixels().GetPixels());
        }

        internal static void AddComponent(Component component)
        {
            _components.Add(component);
        }
    }
}
