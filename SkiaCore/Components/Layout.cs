using SkiaCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using SkiaCore.Common;
using SkiaSharp;

namespace SkiaCore.Components
{
    public abstract class Layout
    {
        public int Id;
        public SKSurface Surface { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        //Data
        public Point Padding { get; set; } = new Point { X = 10, Y = 10};
        public Point ItemPadding { get; set; } = new Point { X = 100, Y = 100 };

        protected List<Component> ComponentStack = new List<Component>();

        //Abstract

        public abstract void CalculateItemsPosition();

        //Methods
        public virtual void AddChild(Component component)
        {
            ComponentStack.Add(component);
        }
        public virtual void RemoveChild(Component component)
        {
            for(int i = 0; i < ComponentStack.Count; i++)
            {
                if(ComponentStack[i].Id == component.Id)
                {
                    ComponentStack.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
