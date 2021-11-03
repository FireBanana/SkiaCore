# SkiaCore - Cross-Platform SkiaSharp Renderer on .Net Core

SkiaCore is a library that uses OpenGL(through the Arqan.x64 GLFW wrapper) to render SkiaSharp in a window. It is currently set-up for Windows only but can be easily expanded with
the alternate Arqan wrappers. This is still in experimental stages and only provides a simple interface for creating UI

<b>Note: This library is currently in Beta</b>

## Getting Started

It's quite easy to get started with SkiaCore. First reference the project into your main project, and then initialize:

```
using SkiaCore;

Core.Initialize(800, 600, "Sprite Editor");
```

Now you can add custom components in such a fashion:
```
Core.AddRenderComponent((surface) => new Rectangle(surface, 0, 0, 800, 50));
Core.AddRenderComponent((surface) => new Button(surface, 10, 10, 100, 25, "Open"));
Core.AddRenderComponent((surface) => new SpriteView(surface, 0, 50, 800, 250));
```

## Components

Components are the building blocks of SkiaSharp. You can implement the abstract class 'Component' from SkiaCore.Components to start building your own. Here's an example of Rectangle
from the example above:

```
using SkiaCore.Components;
using SkiaSharp;

namespace Example
{
    public class Rectangle : Component
    {
        public Rectangle(SKSurface surface, int x, int y, int width, int height, params object[] args) : base(surface, x, y, width, height, args)
        {
            Surface = surface;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        override public void Render()
        {
            Surface.Canvas.DrawRect(X, Y, Width, Height, new SKPaint() { Color = SKColors.Bisque }) ;
        }
    }
}
```

If you would like click functionality, you can implement from 'InteractableComponent' which provides override methods OnClick(), OnMouseEnter(), and OnMouseExit(). Heres an example
of a Button that implements this class and uses P/Invoke to open a Windows dialog to load a file:

```
using System;
using SkiaSharp;
using SkiaCore.Components;
using System.Runtime.InteropServices;

namespace SpriteEditor.Components
{
    public class Button : InteractableComponent
    {
        string _text;
        SKColor _inColor = SKColors.Brown;
        SKColor _outColor = SKColors.BurlyWood;
        SKColor _color;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct OpenFileName
        {
            public int lStructSize;
            public IntPtr hwndOwner;
            public IntPtr hInstance;
            public string lpstrFilter;
            public string lpstrCustomFilter;
            public int nMaxCustFilter;
            public int nFilterIndex;
            public string lpstrFile;
            public int nMaxFile;
            public string lpstrFileTitle;
            public int nMaxFileTitle;
            public string lpstrInitialDir;
            public string lpstrTitle;
            public int Flags;
            public short nFileOffset;
            public short nFileExtension;
            public string lpstrDefExt;
            public IntPtr lCustData;
            public IntPtr lpfnHook;
            public string lpTemplateName;
            public IntPtr pvReserved;
            public int dwReserved;
            public int flagsEx;
        }

        [DllImport("Comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetOpenFileName(ref OpenFileName ofn);

        public Button(SKSurface surface, int x, int y, int width, int height, params object[] args) : base(surface, x, y, width, height, args)
        {
            _text = args[0].ToString();
            _color = _outColor;
        }

        override public void OnClick()
        {
            var ofn = new OpenFileName();
            ofn.lStructSize = Marshal.SizeOf(ofn);
            ofn.lpstrFilter = "All files(*.*)\0\0";
            ofn.lpstrFile = new string(new char[256]);
            ofn.nMaxFile = ofn.lpstrFile.Length;
            ofn.lpstrFileTitle = new string(new char[64]);
            ofn.nMaxFileTitle = ofn.lpstrFileTitle.Length;
            ofn.lpstrTitle = "Open File Dialog...";
            if (GetOpenFileName(ref ofn)) 
            {
                SpriteView.SetImage(ofn.lpstrFile);
            }
        }

        override public void OnMouseEnter()
        {
            _color = _inColor;
        }

        override public void OnMouseExit()
        {
            _color = _outColor;
        }

        override public void Render()
        {
            Surface.Canvas.DrawRect(X, Y, Width, Height, new SKPaint { Color = _color });
            Surface.Canvas.DrawText(_text, X + (Width / 2), Y + (Height / 1.75f), new SKPaint { Color = SKColors.Bisque, TextAlign = SKTextAlign.Center });
        }
    }
}

```

### Random example of this quick UI:
![1](1.PNG)
![2](2.PNG)

## Dependencies
- Arqan (https://github.com/TheBoneJarmer/Arqan)
- SkiaSharp
- Visual Studio 2019
