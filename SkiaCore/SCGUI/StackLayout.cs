using SkiaCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SkiaCore.Common;
using SkiaSharp;

namespace SkiaCore.SCGUI
{
    public class StackLayout : Layout
    {
        public override void CalculateItemsPosition()
        {
            throw new NotImplementedException();
        }

        public override void AddChild(Component component)
        {
            if (ComponentStack.Count == 0)
            {
                component.X = Padding.X;
                component.Y = Padding.Y;
            }
            else
            {
                component.X = Padding.X;
                component.Y = ComponentStack.Last().Y + ComponentStack.Last().Height * 2 + ItemPadding.Y;
            }

            base.AddChild(component);
        }

        public override void RemoveChild(Component component)
        {
            base.RemoveChild(component);
        }
    }
}
