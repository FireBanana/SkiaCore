using Arqan;
using SkiaCore.Common;
using SkiaCore.Components;
using SkiaCore.GL;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkiaCore
{
    public class Window
    {
        internal IntPtr WindowPointer { get; private set; }

        internal readonly int Width;
        internal readonly int Height;
        internal readonly string Title;

        private readonly SkiaCoreOptions _options;
        private readonly GraphicsRenderer _renderer;
        private readonly UIQueue _queue;
        //private readonly InputHandler _inputHandler;
        //private readonly EventSystem _eventSystem;

        internal Window(int width, int height, string title,
            SkiaCoreOptions options = new SkiaCoreOptions())
        {
            Width = width;
            Height = height;

            Title = title;

            _options = options;

            _renderer = new GraphicsRenderer(width, height);
            _queue = new UIQueue();
        }

        public void AddRenderComponent(Component component, Component parent = null)
        {
            _queue.AddToQueue(() =>
            {
                _renderer.AddComponent(component, parent);

                if (component is InteractableComponent)
                    InputHandler.AddComponent(component as InteractableComponent);
            });
        }

        public void Recalculate()
        {
            _queue.AddToQueue(() =>
            {
                _renderer.UpdateLayout();
            });
        }

        internal void SetUpInterfaces()
        {
            GLInterface.InitializeWindow();

            WindowPointer = GLInterface.CreateWindowContext(Width, Height, Title);

            //Events.Initialize(WindowPointer);
            //InputHandler.Initialize(WindowPointer);

            GLInterface.SetUpOptions(_options);
            GLInterface.CreateProgram(_renderer.Surface.PeekPixels().GetPixels(), Width, Height);
        }

        internal void Update()
        {
            while (GLFW.glfwWindowShouldClose(WindowPointer) == 0)
            {
                GLInterface.ActivateContext(WindowPointer);

                _queue.CallDispatch();

                _renderer.Update();
                //InputHandler.Update();

                GLInterface.Draw(WindowPointer);
                GLInterface.Poll();
            }
        }

        internal void ExecuteOnUIThread(Action action)
        {
            _queue.AddToQueue(action);
        }
    }
}
