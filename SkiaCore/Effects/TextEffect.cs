using SkiaCore.Interfaces;
using System;
using SkiaSharp;
using System.Collections.Generic;
using System.Text;
using SkiaCore.Components;

namespace SkiaCore.Effects
{
    public class TextEffect : IEffect
    {
        private readonly string _text;
        private readonly SKPaint _paint;

        private readonly int _xOffset;
        private readonly int _yOffset;

        public TextEffect(string txt, float size, int xOffset = 0, int yOffset = 0)
        {
            _text = txt;
            _paint = new SKPaint();

            _paint.Color = SKColor.Parse("#ff0000");
            _paint.TextSize = size;
            _paint.TextAlign = SKTextAlign.Center;

            _xOffset = xOffset;
            _yOffset = yOffset;
        }

        void IEffect.Draw(Component component, SKCanvas canvas)
        {
            canvas.DrawText(_text, new SKPoint(component.X + _xOffset, component.Y + _yOffset), _paint);
        }
    }
}
