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
    internal class GraphicsRenderer
    {
        internal SKSurface                       Surface { get; private set; }
        internal Component                       Root { get; private set; }

        internal int                             Width { get; private set; } = 800;
        internal int                             Height { get; private set; } = 600;

        private SKImageInfo                      _imageInfo;

        private readonly List<ComponentNodePair> _componentNodeList 
            = new List<ComponentNodePair>();

        public GraphicsRenderer(int width, int height)
        {
            _imageInfo = new SKImageInfo(width, height);

            Surface = SKSurface.Create(_imageInfo);

            Width = width;
            Height = height;
        }

        internal void Update()
        {
            Surface.Canvas.Clear(SKColor.Empty);

            foreach (var cnPair in _componentNodeList)
                cnPair.Component.Render(Surface);
            
            GLInterface.RenderSurface(Surface.PeekPixels().GetPixels(), Width, Height);
        }

        internal void RegisterOnResize(Events e) =>
            e.SetFramebufferResizeCallback((w, h) => { Resize(w, h); });

        internal void AddComponent(Component component, Component parent = null)
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

        internal void Resize(int width, int height)
        {
            _imageInfo.Width = width;
            _imageInfo.Height = height;

            Width = width;
            Height = height;

            Root.Width = width;
            Root.Height = height;

            // Open Skia/Skiasharp source to view how data created
            Surface.Dispose(); 
            Surface = SKSurface.Create(_imageInfo);

            UpdateLayout();
        }

        internal void UpdateLayout()
        {
            Root.CalculateLayout();

            RecalculateTree();
        }

        private void RecalculateTree(YogaNode node = null, float x = 0, float y = 0)
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
