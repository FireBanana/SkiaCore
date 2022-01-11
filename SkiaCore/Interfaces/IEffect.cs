using SkiaCore.Components;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkiaCore.Interfaces
{
    public interface IEffect
    {
        internal void Draw(Component component, SKCanvas canvas);
    }
}
