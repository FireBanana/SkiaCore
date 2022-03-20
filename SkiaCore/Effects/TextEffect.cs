using SkiaCore.Interfaces;
using System;
using SkiaSharp;
using System.Collections.Generic;
using System.Text;
using SkiaCore.Components;
using SkiaCore.Common;

namespace SkiaCore.Effects
{

    /// ======================================
    /// TODO: Add a callback for parent resize
    /// to make text resize if bigger than
    /// width.
    /// ======================================
    public class TextEffect : IEffect
    {
        // Move to types
        public enum TextPosition
        {
            Left,
            Center,
            Right
        }

        private readonly string _text;
        private readonly SKPaint _paint;
        private readonly TextPosition _position;

        public TextEffect(string txt, float size, Color colorHex,
            TextPosition position = TextPosition.Center)
        {
            _text = txt;
            _paint = new SKPaint();

            _paint.Color = colorHex.RawColor;
            _paint.TextSize = size;

            _position = position;

            SetTextAlignment(position);
        }

        private void SetTextAlignment(TextPosition position)
        {
            switch (position)
            {
                case TextPosition.Left:
                    _paint.TextAlign = SKTextAlign.Left;
                    break;

                case TextPosition.Center:
                    _paint.TextAlign = SKTextAlign.Center;
                    break;

                case TextPosition.Right:
                    _paint.TextAlign = SKTextAlign.Right;
                    break;
            }
        }

        private float ResolveTextHorizontalPosition(TextPosition position, Component component)
        {
            switch (position)
            {
                case TextPosition.Left:
                    return 0.0f;

                case TextPosition.Center:
                    return component.Width / 2;

                case TextPosition.Right:
                    return component.Width;

                default:
                    return component.Width / 2;
            }
        }

        void IEffect.Draw(Component component, SKCanvas canvas)
        {

            canvas.DrawText
                (
                    _text,
                    new SKPoint(
                        component.X + ResolveTextHorizontalPosition(_position, component),
                        component.Y + (component.Height / 2) + (_paint.FontMetrics.XHeight / 2)
                    ),
                    _paint
                );
        }
    }
}
