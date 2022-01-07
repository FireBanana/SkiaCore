using Arqan;
using SkiaCore.Components;
using SkiaCore.Common;
using SkiaSharp;
using System.Collections.Generic;
using Facebook.Yoga;
using System.Linq;
using System;
using SkiaCore.GL;

namespace SkiaCore
{
    internal static class GraphicsRenderer
    {
        internal static SKSurface Surface { get; private set; }
        internal static Component Root { get; private set; }

        internal static int Width { get; private set; } = 800;
        internal static int Height { get; private set; } = 600;

        private static SKImageInfo _imageInfo;
        private static List<ComponentNodePair> _componentNodeList = new List<ComponentNodePair>();

        internal static void Initialize
            (int width, int height)
        {
            _imageInfo = new SKImageInfo(width, height);

            Surface = SKSurface.Create(_imageInfo);

            Width = width;
            Height = height;

            Events.SetFramebufferResizeCallback((w, h) => { Resize(w, h); });
        }

        internal static void Update()
        {
            Surface.Canvas.Clear(SKColor.Empty);

            foreach (var cnPair in _componentNodeList)
                cnPair.Component.Render(Surface);

            GL10.glTexImage2D(
                GL11.GL_TEXTURE_2D, 0, GL11.GL_RGB,
                Width, Height, 0, GL12.GL_BGRA,
                GL11.GL_UNSIGNED_BYTE, Surface.PeekPixels().GetPixels()
                );

        }

        internal static void AddComponent(Component component, Component parent = null)
        {
            if (Root == null)
            {
                Root = component;

                //Overrides the roots width and height to fit the window
                Root.Width = Width;
                Root.Height = Height;
            }

            if (!component.Equals(Root))
            {
                if (parent == null)
                    Root.Attach(component);
                else
                    parent.Attach(component);
            }

            _componentNodeList.Add(new ComponentNodePair() { Component = component, Node = component.GetNode() });
        }

        internal static void Resize(int width, int height)
        {
            _imageInfo.Width = width;
            _imageInfo.Height = height;            

            Width = width;
            Height = height;

            Root.Width = width;
            Root.Height = height;

            Surface.Dispose();
            Surface = SKSurface.Create(_imageInfo);

            UpdateLayout();
        }

        internal static void UpdateLayout()
        {
            Root.CalculateLayout();

            RecalculateTree();
        }

        private static void RecalculateTree(YogaNode node = null, float x = 0, float y = 0)
        {
            node = node == null ? Root.GetNode() : node;

            ComponentNodePair currentCNPair = _componentNodeList.First(x => x.Node.Equals(node));

            x += currentCNPair.Node.LayoutX;
            y += currentCNPair.Node.LayoutY;

            currentCNPair.Component.X = (int)x;
            currentCNPair.Component.Y = (int)y;

            for (int i = 0; i < node.Count; ++i)
            {
                RecalculateTree(node[i], x, y);
            }
        }
    }
}
