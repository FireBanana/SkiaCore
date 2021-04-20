using SkiaCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using SkiaCore.Common;
using SkiaSharp;

namespace SkiaCore.SCGUI
{
    public abstract class Layout : Component
    {
        //Data
        public int Padding { get; set; } = 0;
        public List<Component> ComponentStack = new List<Component>();

        //Abstract

        public abstract void CalculateItemsPosition();

        //Methods

        protected Layout(SKSurface surface, int x, int y, int width, int height, params object[] args) : base(surface, x, y, width, height, args)
        {

        }
        public void PushToStack(Component component)
        {
            ComponentStack.Add(component);
        }
        public void RemoveFromStack(Component component)
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
