using Arqan;
using SkiaCore.Components;
using SkiaCore.Common;
using SkiaSharp;
using System.Collections.Generic;
using Facebook.Yoga;
using System.Linq;
using System;

namespace SkiaCore
{
    internal static class GraphicsRenderer
    {
        internal static SKSurface Surface;

        private static List<ComponentNodePair> _componentNodeList = new List<ComponentNodePair>();
        private static SKColor _backgroundColor;
        private static RootComponent _root;

        public static RootComponent GetRoot() => _root;

        internal static void Initialize(SKSurface surface, int width, int height, SKColor color = default(SKColor))
        {
            Surface = surface;
            _backgroundColor = color;

            _root = new RootComponent(width, height);

            AddComponent(_root);
        }

        internal static void Update()
        {
            Surface.Canvas.Clear(_backgroundColor);

            foreach (var cnPair in _componentNodeList)
                cnPair.Component.Render(Surface);

            GL10.glTexImage2D(
                GL11.GL_TEXTURE_2D, 0, GL11.GL_RGB,
                Core.Width, Core.Height, 0, GL12.GL_BGRA,
                GL11.GL_UNSIGNED_BYTE, Surface.PeekPixels().GetPixels()
                );

        }

        internal static void AddComponent(Component component, Component parent = null)
        {
            if (!component.Equals(_root))
            {
                if (parent == null)
                    _root.Attach(component);
                else
                    parent.Attach(component);
            }

            _componentNodeList.Add(new ComponentNodePair() { Component = component, Node = component.GetNode() });
        }

        internal static void UpdateLayout()
        {
            _root.CalculateLayout();

            RecalculateTree();
        }

        private static void RecalculateTree(YogaNode node = null, float x = 0, float y = 0)
        {
            node = node == null ? _root.GetNode() : node;

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
